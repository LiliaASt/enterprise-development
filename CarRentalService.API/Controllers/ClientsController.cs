using CarRentalService.Application.Contracts.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.API.Controllers;

/// <summary>
/// Controller for managing clients
/// </summary>
/// <param name="service">Client service dependency</param>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController(IClientService service) : ControllerBase
{
    private readonly IClientService _service = service;

    /// <summary>
    /// Retrieves all clients
    /// </summary>
    /// <returns>List of all clients</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDto>>> GetAll()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific client by ID
    /// </summary>
    /// <param name="id">Client identifier</param>
    /// <returns>Client details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var client = await _service.Get(id);
        if (client == null)
        {
            return NotFound($"Client with ID {id} not found.");
        }
        return Ok(client);
    }

    /// <summary>
    /// Creates a new client
    /// </summary>
    /// <param name="dto">Client creation data</param>
    /// <returns>Created client</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClientDto>> Create([FromBody] ClientCreateUpdateDto dto)
    {
        try
        {
            var createdClient = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdClient.Id }, createdClient);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid client data: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    /// <param name="id">Client identifier</param>
    /// <param name="dto">Client update data</param>
    /// <returns>No content on success</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] ClientCreateUpdateDto dto)
    {
        try
        {
            var result = await _service.Update(dto, id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Client with ID {id} not found.");
        }
    }

    /// <summary>
    /// Deletes a client
    /// </summary>
    /// <param name="id">Client identifier</param>
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
