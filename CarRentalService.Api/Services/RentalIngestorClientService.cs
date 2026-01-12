using AutoMapper;
using CarRentalService.Application.Contracts.Grpc;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Api.Configuration;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CarRentalService.Api.Services;

/// <summary>
/// Background gRPC client service for receiving rental contracts
/// </summary>
public class CarRentalGrpcClient(
    RentalIngestor.RentalIngestorClient client,
    IServiceScopeFactory scopeFactory,
    IMapper mapper,
    ILogger<CarRentalGrpcClient> logger,
    IOptions<RentalGeneratorOptions> options,
    IMemoryCache cache
) : BackgroundService
{
    private static readonly TimeSpan _cacheTtl = TimeSpan.FromMinutes(10);
    private readonly RentalGeneratorOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("CarRentalGrpcClient service starting...");

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndProcessAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (RpcException ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "gRPC stream error: {StatusCode} - {StatusDetail}",
                    ex.StatusCode, ex.Status.Detail);
                await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelay), stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Unexpected error in CarRentalGrpcClient");
                await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelay), stoppingToken);
            }
        }
    }

    private async Task ConnectAndProcessAsync(CancellationToken stoppingToken)
    {
        var count = _options.Count;
        var batchSize = _options.BatchSize;

        logger.LogInformation("Connecting to RentalGenerator gRPC service...");

        using var call = client.StreamRentals(cancellationToken: stoppingToken);
        var requestId = Guid.NewGuid().ToString("N");

        var writerTask = Task.Run(async () =>
        {
            await call.RequestStream.WriteAsync(new RentalGenerationRequest
            {
                RequestId = requestId,
                Count = count,
                BatchSize = batchSize
            }, stoppingToken);

            await call.RequestStream.CompleteAsync();
        }, stoppingToken);

        await foreach (var batch in call.ResponseStream.ReadAllAsync(stoppingToken))
        {
            if (batch.RequestId != requestId)
                continue;

            await ProcessBatchAsync(batch, stoppingToken);

            if (batch.IsFinal)
            {
                logger.LogInformation("Finished receiving rentals for RequestId={RequestId}", requestId);
                break;
            }
        }

        await writerTask;
    }

    private async Task ProcessBatchAsync(RentalBatchStreamMessage batch, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();

        var rentalService = scope.ServiceProvider.GetRequiredService<IRentService>();
        var carService = scope.ServiceProvider.GetRequiredService<ICarService>();
        var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();

        var validRentals = new List<RentCreateUpdateDto>();

        foreach (var rental in batch.Rentals)
        {
            if (!await ValidateEntityExistsAsync(rental.CarId, "Car",
                async (id, token) => await carService.Get(id) != null, ct))
                continue;

            if (!await ValidateEntityExistsAsync(rental.CustomerId, "Client",
                async (id, token) => await clientService.Get(id) != null, ct))
                continue;

            var dto = mapper.Map<RentCreateUpdateDto>(rental);
            validRentals.Add(dto);
        }

        var createdCount = 0;
        foreach (var rentalDto in validRentals)
        {
            try
            {
                await rentalService.Create(rentalDto);
                createdCount++;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to create rental for CarId={CarId}, ClientId={ClientId}",
                    rentalDto.CarId, rentalDto.ClientId);
            }
        }

        logger.LogInformation(
            "Processed batch: Total={Total}, Valid={Valid}, Created={Created}, IsFinal={IsFinal}",
            batch.Rentals.Count, validRentals.Count, createdCount, batch.IsFinal);
    }

    private async Task<bool> ValidateEntityExistsAsync<TId>(
        TId id,
        string entityName,
        Func<TId, CancellationToken, Task<bool>> existenceCheck,
        CancellationToken ct)
    {
        var cacheKey = $"{entityName}:exists:{id}";

        if (cache.TryGetValue(cacheKey, out bool cached))
            return cached;

        bool exists;
        try
        {
            exists = await existenceCheck(id, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning(ex, "Error checking existence of {Entity} with id {Id}", entityName, id);
            exists = false;
        }
        catch (OperationCanceledException)
        {
            throw;
        }

        cache.Set(cacheKey, exists, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheTtl
        });

        return exists;
    }
}
