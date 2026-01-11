using CarRentalService.Application.Contracts.CarModelGeneration;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// Controller for managing car model generations
/// </summary>
/// <param name="service">Car model generation service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CarModelGenerationsController(ICarModelGenerationService service) : ControllerBase
{
    private readonly ICarModelGenerationService _service = service;

    /// <summary>
    /// Retrieves all car model generations
    /// </summary>
    /// <returns>List of all car model generations</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<CarModelGenerationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarModelGenerationDto>>> GetAll()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific car model generation by ID
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>Car model generation details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CarModelGenerationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarModelGenerationDto>> GetById(int id)
    {
        var generation = await _service.Get(id);
        if (generation == null)
        {
            return NotFound($"CarModelGeneration with ID {id} not found.");
        }
        return Ok(generation);
    }

    /// <summary>
    /// Creates a new car model generation
    /// </summary>
    /// <param name="dto">Car model generation creation data</param>
    /// <returns>Created car model generation</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CarModelGenerationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarModelGenerationDto>> Create([FromBody] CarModelGenerationCreateUpdateDto dto)
    {
        try
        {
            var createdGeneration = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdGeneration.Id }, createdGeneration);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid car model generation data: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing car model generation
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <param name="dto">Car model generation update data</param>
    /// <returns>No content on success</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] CarModelGenerationCreateUpdateDto dto)
    {
        try
        {
            await _service.Update(dto, id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"CarModelGeneration with ID {id} not found.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a car model generation
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        return result ? NoContent() : NotFound();
    }
}
