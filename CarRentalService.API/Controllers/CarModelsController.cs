using CarRentalService.Application.Contracts.CarModel;
using CarRentalService.Application.Contracts.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// Controller for managing car models
/// </summary>
/// <param name="service">Car model service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CarModelsController(ICarModelService service) : ControllerBase
{
    private readonly ICarModelService _service = service;

    /// <summary>
    /// Retrieves all car models
    /// </summary>
    /// <returns>List of all car models</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<CarModelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarModelDto>>> GetAll()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific car model by ID
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>Car model details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CarModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarModelDto>> GetById(int id)
    {
        var carModel = await _service.Get(id);
        if (carModel == null)
        {
            return NotFound($"CarModel with ID {id} not found.");
        }
        return Ok(carModel);
    }

    /// <summary>
    /// Creates a new car model
    /// </summary>
    /// <param name="dto">Car model creation data</param>
    /// <returns>Created car model</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CarModelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarModelDto>> Create([FromBody] CarModelCreateUpdateDto dto)
    {
        try
        {
            var createdCarModel = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdCarModel.Id }, createdCarModel);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid car model data: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing car model
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <param name="dto">Car model update data</param>
    /// <returns>No content on success</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] CarModelCreateUpdateDto dto)
    {
        try
        {
            await _service.Update(dto, id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"CarModel with ID {id} not found.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a car model
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves all cars associated with a specific car model
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>List of cars belonging to the specified model</returns>
    [HttpGet("{id:int}/cars")]
    [ProducesResponseType(typeof(List<CarDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarDto>>> GetCarsByModel(int id)
    {
        var result = await _service.GetCarsByModelAsync(id);
        return Ok(result);
    }
}
