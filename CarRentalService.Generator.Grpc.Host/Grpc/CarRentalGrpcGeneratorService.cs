using CarRentalService.Application.Contracts.Grpc;
using CarRentalService.Generator.Grpc.Host.Generator;
using Grpc.Core;

namespace CarRentalService.Generator.Grpc.Host.Grpc;

/// <summary>
/// gRPC service for generating rental contracts
/// </summary>
/// <param name="configuration">Application configuration</param>
/// <param name="logger">Logger instance</param>
public sealed class CarRentalGrpcGeneratorService(
    IConfiguration configuration,
    ILogger<CarRentalGrpcGeneratorService> logger
) : RentalIngestor.RentalIngestorBase
{
    private readonly int _defaultBatchSize = configuration.GetValue("Generator:BatchSize", 10);
    private readonly int _waitTimeSeconds = configuration.GetValue("Generator:WaitTime", 2);
    private readonly bool _enableLogging = configuration.GetValue("Generator:EnableLogging", true);

    /// <summary>
    /// Bidirectional gRPC stream method for generating rental contracts
    /// </summary>
    /// <param name="requestStream">Incoming request stream</param>
    /// <param name="responseStream">Outgoing response stream</param>
    /// <param name="context">Server call context</param>
    public override async Task StreamRentals(
        IAsyncStreamReader<RentalGenerationRequest> requestStream,
        IServerStreamWriter<RentalBatchStreamMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogInformation("Starting gRPC stream for rental generation");

        await foreach (var req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            if (_enableLogging)
            {
                logger.LogInformation(
                    "Received generation request: RequestId={RequestId}, Count={Count}, BatchSize={BatchSize}",
                    req.RequestId, req.Count, req.BatchSize);
            }

            var batchSize = req.BatchSize > 0 ? req.BatchSize : _defaultBatchSize;
            var sent = 0;

            while (sent < req.Count && !context.CancellationToken.IsCancellationRequested)
            {
                var take = Math.Min(batchSize, req.Count - sent);
                var rentals = RentalGenerator.Generate(take);

                var payload = new RentalBatchStreamMessage
                {
                    RequestId = req.RequestId,
                    IsFinal = sent + take >= req.Count
                };

                payload.Rentals.AddRange(rentals);

                if (_enableLogging)
                {
                    logger.LogDebug(
                        "Sending batch: RequestId={RequestId}, BatchSize={BatchSize}, TotalSent={Sent}, IsFinal={IsFinal}",
                        req.RequestId, take, sent + take, payload.IsFinal);
                }

                await responseStream.WriteAsync(payload, context.CancellationToken);
                sent += take;

                if (!payload.IsFinal && _waitTimeSeconds > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_waitTimeSeconds), context.CancellationToken);
                }
            }

            if (_enableLogging)
            {
                logger.LogInformation(
                    "Completed generation for RequestId={RequestId}: TotalGenerated={Total}",
                    req.RequestId, sent);
            }
        }
    }
}
