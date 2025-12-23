using CarRentalService.API.DTOs.Requests;
using CarRentalService.API.DTOs.Responses;
using CarRentalService.API.Interfaces;
using CarRentalService.Domain.Data;
using CarRentalService.Domain.Models;
using Mapster;

namespace CarRentalService.API.Services;

/// <summary>
/// Client service implementation
/// </summary>
public class ClientService : IApplicationService<ClientDto, ClientCreateUpdateDto>
{
    private readonly List<Customer> _customers;
    private int _nextId;

    public ClientService(TestData testData)
    {
        _customers = testData.Customers.ToList();
        _nextId = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
    }

    public List<ClientDto> ReadAll()
    {
        return _customers.Adapt<List<ClientDto>>();
    }

    public ClientDto? Read(int id)
    {
        var client = _customers.FirstOrDefault(c => c.Id == id);
        return client?.Adapt<ClientDto>();
    }

    public ClientDto? Create(ClientCreateUpdateDto dto)
    {
        var client = new Customer
        {
            Id = _nextId++,
            DriverLicenseNumber = dto.DriverLicenseNumber,
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth
        };

        _customers.Add(client);
        return client.Adapt<ClientDto>();
    }

    public bool Update(ClientCreateUpdateDto dto, int id)
    {
        var client = _customers.FirstOrDefault(c => c.Id == id);
        if (client == null) return false;

        client.DriverLicenseNumber = dto.DriverLicenseNumber;
        client.FullName = dto.FullName;
        client.DateOfBirth = dto.DateOfBirth;

        return true;
    }

    public bool Delete(int id)
    {
        var client = _customers.FirstOrDefault(c => c.Id == id);
        if (client == null) return false;

        return _customers.Remove(client);
    }
}
