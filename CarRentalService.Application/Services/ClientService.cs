using AutoMapper;
using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application.Services;

/// <summary>
/// Service implementation for managing clients
/// </summary>
/// <param name="customerRepository">Customer repository</param>
/// <param name="mapper">AutoMapper instance</param>
public class ClientService(
    IRepository<Customer, int> customerRepository,
    IMapper mapper
) : IClientService
{
    /// <summary>
    /// Creates a new client
    /// </summary>
    /// <param name="dto">Client creation data</param>
    /// <returns>Created client DTO</returns>
    public async Task<ClientDto> Create(ClientCreateUpdateDto dto)
    {
        var allCustomers = await customerRepository.ReadAll();
        var maxId = allCustomers.Any() ? allCustomers.Max(c => c.Id) : 0;

        var customer = mapper.Map<Customer>(dto);
        customer.Id = maxId + 1;

        var created = await customerRepository.Create(customer);
        return mapper.Map<ClientDto>(created);
    }

    /// <summary>
    /// Deletes a client by its identifier
    /// </summary>
    /// <param name="id">Client identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
        => await customerRepository.Delete(id);

    /// <summary>
    /// Retrieves a client by its identifier
    /// </summary>
    /// <param name="id">Client identifier</param>
    /// <returns>Client DTO if found, null otherwise</returns>
    public async Task<ClientDto?> Get(int id)
    {
        var customer = await customerRepository.Read(id);
        return customer != null ? mapper.Map<ClientDto>(customer) : null;
    }

    /// <summary>
    /// Retrieves all clients
    /// </summary>
    /// <returns>List of all client DTOs</returns>
    public async Task<IList<ClientDto>> GetAll()
    {
        var customers = await customerRepository.ReadAll();
        return mapper.Map<List<ClientDto>>(customers);
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    /// <param name="dto">Client update data</param>
    /// <param name="id">Client identifier</param>
    /// <returns>Updated client DTO</returns>
    public async Task<ClientDto> Update(ClientCreateUpdateDto dto, int id)
    {
        var customer = await customerRepository.Read(id);
        if (customer == null)
            throw new KeyNotFoundException($"Client with id {id} not found");

        mapper.Map(dto, customer);
        var updated = await customerRepository.Update(customer);
        return mapper.Map<ClientDto>(updated);
    }
}
