using CarRentalService.Application.Contracts;
using CarRentalService.Application.Contracts.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.Api.Controllers;

/// <summary>
/// Controller for analytics and business intelligence endpoints
/// </summary>
/// <param name="analyticsService">Analytics service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Retrieves customers who rented cars of a specific model name
    /// </summary>
    /// <param name="modelName">Car model name to filter by</param>
    /// <returns>List of customer names</returns>
    [HttpGet("clients-by-model-name")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> GetClientsByModelName([FromQuery] string modelName)
    {
        var result = await analyticsService.ReadCustomersByModelName(modelName);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves customers who rented cars of a specific model ID
    /// </summary>
    /// <param name="modelId">Car model ID to filter by</param>
    /// <returns>List of customer names</returns>
    [HttpGet("clients-by-model-id/{modelId:int}")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> GetClientsByModelId(int modelId)
    {
        var result = await analyticsService.ReadCustomersByModelId(modelId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves cars currently in rent at a specific time
    /// </summary>
    /// <param name="atTime">Time to check for active rentals (default: current time)</param>
    /// <returns>List of currently rented cars</returns>
    [HttpGet("cars-in-rent")]
    [ProducesResponseType(typeof(List<CarRentalResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarRentalResponse>>> GetCarsInRent([FromQuery] DateTime? atTime = null)
    {
        var checkTime = atTime ?? DateTime.Now;
        var result = await analyticsService.ReadCarsInRent(checkTime);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves top N most frequently rented cars
    /// </summary>
    /// <param name="count">Number of top cars to return (default: 5)</param>
    /// <returns>List of top rented cars with statistics</returns>
    [HttpGet("top-rented-cars")]
    [ProducesResponseType(typeof(List<TopCarResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TopCarResponse>>> GetTopRentedCars([FromQuery] int count = 5)
    {
        var result = await analyticsService.ReadTopMostRentedCars(count);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves rental count for all cars
    /// </summary>
    /// <returns>List of all cars with their rental counts</returns>
    [HttpGet("all-cars-with-rental-count")]
    [ProducesResponseType(typeof(List<CarRentalCountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarRentalCountResponse>>> GetAllCarsWithRentalCount()
    {
        var result = await analyticsService.ReadAllCarsWithRentalCount();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves top N customers by total rental revenue
    /// </summary>
    /// <param name="count">Number of top customers to return (default: 5)</param>
    /// <returns>List of top customers by revenue</returns>
    [HttpGet("top-customers-by-revenue")]
    [ProducesResponseType(typeof(List<TopCustomerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TopCustomerResponse>>> GetTopCustomersByRevenue([FromQuery] int count = 5)
    {
        var result = await analyticsService.ReadTopCustomersByTotalAmount(count);
        return Ok(result);
    }
}
