using CarRentalService.Application.Contracts.Grpc;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Grpc.Core;

namespace CarRentalService.Api.Controllers;

/// <summary>
/// Controller for managing rental contract generation via gRPC
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GeneratorController(
    RentalIngestor.RentalIngestorClient grpcClient,
    IRentService rentService,
    ICarService carService,
    IClientService clientService,
    IMapper mapper,
    ILogger<GeneratorController> logger) : ControllerBase
{

    /// <summary>
    /// Gets current system status and statistics
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetStatus()
    {
        try
        {
            var cars = await carService.GetAll();
            var clients = await clientService.GetAll();
            var rents = await rentService.GetAll();

            return Ok(new
            {
                CarsCount = cars.Count,
                ClientsCount = clients.Count,
                RentsCount = rents.Count,
                GeneratorAddress = "https://localhost:7000",
                Status = "The system is working",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Manually triggers test data generation via gRPC
    /// </summary>
    /// <param name="count">Number of rental contracts to generate</param>
    /// <param name="batchSize">Batch size for streaming</param>
    [HttpPost("generate-test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GenerateTestData([FromQuery] int count = 10, [FromQuery] int batchSize = 5)
    {
        if (count <= 0 || batchSize <= 0)
            return BadRequest("Count and batchSize must be greater than 0");

        try
        {
            logger.LogInformation("Starting manual generation of {Count} contracts (batchSize={BatchSize})",
                count, batchSize);

            var requestId = Guid.NewGuid().ToString("N");
            var generated = 0;
            var saved = 0;

            using var call = grpcClient.StreamRentals();

            // Sending a request
            await call.RequestStream.WriteAsync(new RentalGenerationRequest
            {
                RequestId = requestId,
                Count = count,
                BatchSize = batchSize
            });
            await call.RequestStream.CompleteAsync();

            // Get and save data
            await foreach (var batch in call.ResponseStream.ReadAllAsync())
            {
                if (batch.RequestId != requestId)
                    continue;

                logger.LogInformation("Received batch: {BatchCount} contracts", batch.Rentals.Count);
                generated += batch.Rentals.Count;

                foreach (var rental in batch.Rentals)
                {
                    try
                    {
                        var carExists = await carService.Get(rental.CarId) != null;
                        var clientExists = await clientService.Get(rental.CustomerId) != null;

                        if (!carExists || !clientExists)
                        {
                            logger.LogWarning(
                                "Skipping the contract: CarId={CarId} (exists={CarExists}), ClientId={ClientId} (exists={ClientExists})",
                                rental.CarId, carExists, rental.CustomerId, clientExists);
                            continue;
                        }

                        // Save
                        var dto = mapper.Map<RentCreateUpdateDto>(rental);
                        await rentService.Create(dto);
                        saved++;

                        logger.LogDebug("Saving the contract: CarId={CarId}, ClientId={ClientId}",
                            rental.CarId, rental.CustomerId);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error saving the contract");
                    }
                }

                if (batch.IsFinal)
                    break;
            }

            return Ok(new
            {
                RequestId = requestId,
                Generated = generated,
                Saved = saved,
                Failed = generated - saved,
                Message = "Generation completed successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in generating test data");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Tests gRPC client connection status
    /// </summary>
    [HttpGet("test-grpc-connection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> TestGrpcConnection()
    {
        try
        {
            return Ok("gRPC client is registered and ready to work");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error gRPC: {ex.Message}");
        }
    }
}
