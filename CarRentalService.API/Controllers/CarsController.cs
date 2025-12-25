using CarRentalService.Application.Contracts.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// API controller for managing cars
/// </summary>
/// <param name="service">Car service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CarsController(ICarService service) : ControllerBase
{
    private readonly ICarService _service = service;

    /// <summary>
    /// Get all cars
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CarDto>), StatusCodes.Status200OK)]
    public ActionResult<List<CarDto>> GetAll()
    {
        return Ok(_service.ReadAll());
    }

    /// <summary>
    /// Get car by ID
    /// </summary>
    /// <param name="id">Car identifier</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<CarDto> GetById(int id)
    {
        var car = _service.Read(id);
        if (car == null)
        {
            return NotFound($"Car with ID {id} not found.");
        }
        return Ok(car);
    }

    /// <summary>
    /// Create new car
    /// </summary>
    /// <param name="dto">Car creation data</param>
    [HttpPost]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ActionResult<CarDto> Create([FromBody] CarCreateUpdateDto dto)
    {
        var createdCar = _service.Create(dto);
        if (createdCar == null)
        {
            return BadRequest("Invalid car data or CarModelGenerationId not found.");
        }
        return CreatedAtAction(nameof(GetById), new { id = createdCar.Id }, createdCar);
    }

    /// <summary>
    /// Update existing car
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <param name="dto">Car update data</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Update(int id, [FromBody] CarCreateUpdateDto dto)
    {
        var result = _service.Update(dto, id);
        if (!result)
        {
            return NotFound($"Car with ID {id} not found or invalid data.");
        }
        return NoContent();
    }

    /// <summary>
    /// Delete car
    /// </summary>
    /// <param name="id">Car identifier</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Delete(int id)
    {
        var result = _service.Delete(id);
        return result ? NoContent() : Ok();
    }
}
