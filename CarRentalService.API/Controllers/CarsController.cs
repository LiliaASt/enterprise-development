using CarRentalService.Application.Contracts.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.Api.Controllers;

/// <summary>
/// Controller for managing cars
/// </summary>
/// <param name="service">Car service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CarsController(ICarService service) : ControllerBase
{
    /// <summary>
    /// Retrieves all cars
    /// </summary>
    /// <returns>List of all cars</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<CarDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarDto>>> GetAll()
    {
        var result = await service.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific car by ID
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>Car details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarDto>> GetById(int id)
    {
        var car = await service.Get(id);
        if (car == null)
        {
            return NotFound($"Car with ID {id} not found.");
        }
        return Ok(car);
    }

    /// <summary>
    /// Creates a new car
    /// </summary>
    /// <param name="dto">Car creation data</param>
    /// <returns>Created car</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarDto>> Create([FromBody] CarCreateUpdateDto dto)
    {
        try
        {
            var createdCar = await service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdCar.Id }, createdCar);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid car data: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing car
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <param name="dto">Car update data</param>
    /// <returns>No content on success</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] CarCreateUpdateDto dto)
    {
        try
        {
            var result = await service.Update(dto, id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Car with ID {id} not found.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a car
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await service.Delete(id);
        return result ? NoContent() : NotFound();
    }
}
