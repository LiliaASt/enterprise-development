using CarRentalService.Application.Contracts.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// API controller for managing clients
/// </summary>
/// <param name="service">Client service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController(IClientService service) : ControllerBase
{
    private readonly IClientService _service = service;

    /// <summary>
    /// Get all clients
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    public ActionResult<List<ClientDto>> GetAll()
    {
        return Ok(_service.ReadAll());
    }

    /// <summary>
    /// Get client by ID
    /// </summary>
    /// <param name="id">Client identifier</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<ClientDto> GetById(int id)
    {
        var client = _service.Read(id);
        if (client == null)
        {
            return NotFound($"Client with ID {id} not found.");
        }
        return Ok(client);
    }

    /// <summary>
    /// Create new client
    /// </summary>
    /// <param name="dto">Client creation data</param>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ActionResult<ClientDto> Create([FromBody] ClientCreateUpdateDto dto)
    {
        var createdClient = _service.Create(dto);
        if (createdClient == null)
        {
            return BadRequest("Invalid client data.");
        }
        return CreatedAtAction(nameof(GetById), new { id = createdClient.Id }, createdClient);
    }

    /// <summary>
    /// Update existing client
    /// </summary>
    /// <param name="id">Client identifier</param>
    /// <param name="dto">Client update data</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Update(int id, [FromBody] ClientCreateUpdateDto dto)
    {
        var result = _service.Update(dto, id);
        if (!result)
        {
            return NotFound($"Client with ID {id} not found.");
        }
        return NoContent();
    }

    /// <summary>
    /// Delete client
    /// </summary>
    /// <param name="id">Client identifier</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Delete(int id)
    {
        var result = _service.Delete(id);
        return result ? NoContent() : Ok();
    }
}
