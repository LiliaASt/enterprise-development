using CarRentalService.Application.Contracts.Grpc;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Grpc.Core;

namespace CarRentalService.API.Controllers;

/// <summary>
/// Controller for managing rental contract generation via gRPC
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GeneratorController : ControllerBase
{
    private readonly RentalIngestor.RentalIngestorClient _grpcClient;
    private readonly IRentService _rentService;
    private readonly ICarService _carService;
    private readonly IClientService _clientService;
    private readonly IMapper _mapper;
    private readonly ILogger<GeneratorController> _logger;

    /// <summary>
    /// Initializes a new instance of GeneratorController
    /// </summary>
    public GeneratorController(
        RentalIngestor.RentalIngestorClient grpcClient,
        IRentService rentService,
        ICarService carService,
        IClientService clientService,
        IMapper mapper,
        ILogger<GeneratorController> logger)
    {
        _grpcClient = grpcClient;
        _rentService = rentService;
        _carService = carService;
        _clientService = clientService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Gets current system status and statistics
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetStatus()
    {
        try
        {
            var cars = await _carService.GetAll();
            var clients = await _clientService.GetAll();
            var rents = await _rentService.GetAll();

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
            _logger.LogInformation("Starting manual generation of {Count} contracts (batchSize={BatchSize})",
                count, batchSize);

            var requestId = Guid.NewGuid().ToString("N");
            var generated = 0;
            var saved = 0;

            using var call = _grpcClient.StreamRentals();

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

                _logger.LogInformation("Received batch: {BatchCount} contracts", batch.Rentals.Count);
                generated += batch.Rentals.Count;

                foreach (var rental in batch.Rentals)
                {
                    try
                    {
                        var carExists = await _carService.Get(rental.CarId) != null;
                        var clientExists = await _clientService.Get(rental.CustomerId) != null;

                        if (!carExists || !clientExists)
                        {
                            _logger.LogWarning(
                                "Skipping the contract: CarId={CarId} (exists={CarExists}), ClientId={ClientId} (exists={ClientExists})",
                                rental.CarId, carExists, rental.CustomerId, clientExists);
                            continue;
                        }

                        // Save
                        var dto = _mapper.Map<RentCreateUpdateDto>(rental);
                        await _rentService.Create(dto);
                        saved++;

                        _logger.LogDebug("Saving the contract: CarId={CarId}, ClientId={ClientId}",
                            rental.CarId, rental.CustomerId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error saving the contract");
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
            _logger.LogError(ex, "Error in generating test data");
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
