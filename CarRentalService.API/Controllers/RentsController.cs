using CarRentalService.Application.Contracts.Rents;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// API controller for managing rents
/// </summary>
/// <param name="service">Rent service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RentsController(IRentService service) : ControllerBase
{
    private readonly IRentService _service = service;

    /// <summary>
    /// Get all rents
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RentDto>), StatusCodes.Status200OK)]
    public ActionResult<List<RentDto>> GetAll()
    {
        return Ok(_service.ReadAll());
    }

    /// <summary>
    /// Get rent by ID
    /// </summary>
    /// <param name="id">Rent identifier</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<RentDto> GetById(int id)
    {
        var rent = _service.Read(id);
        if (rent == null)
        {
            return NotFound($"Rent with ID {id} not found.");
        }
        return Ok(rent);
    }

    /// <summary>
    /// Create new rent
    /// </summary>
    /// <param name="dto">Rent creation data</param>
    [HttpPost]
    [ProducesResponseType(typeof(RentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ActionResult<RentDto> Create([FromBody] RentCreateUpdateDto dto)
    {
        var createdRent = _service.Create(dto);
        if (createdRent == null)
        {
            return BadRequest("Invalid rent data. Car or Client not found.");
        }
        return CreatedAtAction(nameof(GetById), new { id = createdRent.Id }, createdRent);
    }

    /// <summary>
    /// Update existing rent
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <param name="dto">Rent update data</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Update(int id, [FromBody] RentCreateUpdateDto dto)
    {
        var result = _service.Update(dto, id);
        if (!result)
        {
            return NotFound($"Rent with ID {id} not found or invalid data.");
        }
        return NoContent();
    }

    /// <summary>
    /// Delete rent
    /// </summary>
    /// <param name="id">Rent identifier</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Delete(int id)
    {
        var result = _service.Delete(id);
        return result ? NoContent() : Ok();
    }
}
