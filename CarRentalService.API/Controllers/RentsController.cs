using CarRentalService.Application.Contracts.Rents;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.Api.Controllers;

/// <summary>
/// Controller for managing rental transactions
/// </summary>
/// <param name="service">Rent service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RentsController(IRentService service) : ControllerBase
{
    /// <summary>
    /// Retrieves all rental transactions
    /// </summary>
    /// <returns>List of all rental transactions</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<RentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RentDto>>> GetAll()
    {
        var result = await service.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific rental transaction by ID
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <returns>Rental transaction details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentDto>> GetById(int id)
    {
        var rent = await service.Get(id);
        if (rent == null)
        {
            return NotFound($"Rent with ID {id} not found.");
        }
        return Ok(rent);
    }

    /// <summary>
    /// Creates a new rental transaction
    /// </summary>
    /// <param name="dto">Rental creation data</param>
    /// <returns>Created rental transaction</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentDto>> Create([FromBody] RentCreateUpdateDto dto)
    {
        try
        {
            var createdRent = await service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdRent.Id }, createdRent);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid rent data: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing rental transaction
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <param name="dto">Rental update data</param>
    /// <returns>No content on success</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] RentCreateUpdateDto dto)
    {
        try
        {
            var result = await service.Update(dto, id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Rent with ID {id} not found.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a rental transaction
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await service.Delete(id);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves all rental transactions for a specific client
    /// </summary>
    /// <param name="clientId">Client identifier</param>
    /// <returns>List of rental transactions for the client</returns>
    [HttpGet("by-client/{clientId:int}")]
    [ProducesResponseType(typeof(List<RentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RentDto>>> GetByClient(int clientId)
    {
        var result = await service.GetRentalsByClientAsync(clientId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all rental transactions for a specific car
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>List of rental transactions for the car</returns>
    [HttpGet("by-car/{carId:int}")]
    [ProducesResponseType(typeof(List<RentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RentDto>>> GetByCar(int carId)
    {
        var result = await service.GetRentalsByCarAsync(carId);
        return Ok(result);
    }
}
