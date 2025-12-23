using CarRentalService.API.DTOs.Responses;
using CarRentalService.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// Analytics controller - provides business intelligence endpoints
/// Based on unit tests from first lab work
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Get customers who rented cars of specific model name
    /// </summary>
    /// <param name="modelName">Car model name (e.g., "Toyota Camry")</param>
    [HttpGet("clients-by-model-name")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public ActionResult<List<string>> GetClientsByModelName([FromQuery] string modelName)
    {
        var result = _analyticsService.ReadCustomersByModelName(modelName);
        return Ok(result);
    }

    /// <summary>
    /// Get customers who rented cars of specific model ID
    /// </summary>
    /// <param name="modelId">Car model ID</param>
    [HttpGet("clients-by-model-id/{modelId:int}")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public ActionResult<List<string>> GetClientsByModelId(int modelId)
    {
        var result = _analyticsService.ReadCustomersByModelId(modelId);
        return Ok(result);
    }

    /// <summary>
    /// Get currently rented cars at specific time
    /// </summary>
    /// <param name="atTime">Check time (default: current time)</param>
    [HttpGet("cars-in-rent")]
    [ProducesResponseType(typeof(List<CarRentalResponse>), StatusCodes.Status200OK)]
    public ActionResult<List<CarRentalResponse>> GetCarsInRent([FromQuery] DateTime? atTime = null)
    {
        var checkTime = atTime ?? DateTime.Now;
        var result = _analyticsService.ReadCarsInRent(checkTime);
        return Ok(result);
    }

    /// <summary>
    /// Get top N most rented cars
    /// </summary>
    /// <param name="count">Number of top cars to return (default: 5)</param>
    [HttpGet("top-rented-cars")]
    [ProducesResponseType(typeof(List<TopCarResponse>), StatusCodes.Status200OK)]
    public ActionResult<List<TopCarResponse>> GetTopRentedCars([FromQuery] int count = 5)
    {
        var result = _analyticsService.ReadTopMostRentedCars(count);
        return Ok(result);
    }

    /// <summary>
    /// Get rental count for all cars
    /// </summary>
    [HttpGet("all-cars-with-rental-count")]
    [ProducesResponseType(typeof(List<CarRentalCountResponse>), StatusCodes.Status200OK)]
    public ActionResult<List<CarRentalCountResponse>> GetAllCarsWithRentalCount()
    {
        var result = _analyticsService.ReadAllCarsWithRentalCount();
        return Ok(result);
    }

    /// <summary>
    /// Get top N customers by total rental revenue
    /// </summary>
    /// <param name="count">Number of top customers to return (default: 5)</param>
    [HttpGet("top-customers-by-revenue")]
    [ProducesResponseType(typeof(List<TopCustomerResponse>), StatusCodes.Status200OK)]
    public ActionResult<List<TopCustomerResponse>> GetTopCustomersByRevenue([FromQuery] int count = 5)
    {
        var result = _analyticsService.ReadTopCustomersByTotalAmount(count);
        return Ok(result);
    }
}
