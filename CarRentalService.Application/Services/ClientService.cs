using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Domain.Models;
using CarRentalService.Domain.TestData;

namespace CarRentalService.Application.Services;

/// <summary>
/// Client service implementation
/// </summary>
/// <param name="testData">Test data provider for in-memory storage</param>
public class ClientService(TestData testData) : IClientService
{
    private readonly TestData _testData = testData;

    /// <summary>
    /// Returns all clients
    /// </summary>
    public List<ClientDto> ReadAll()
    {
        return _testData.Customers.Select(c => new ClientDto
        {
            Id = c.Id,
            DriverLicenseNumber = c.DriverLicenseNumber,
            FullName = c.FullName,
            DateOfBirth = c.DateOfBirth
        }).ToList();
    }

    /// <summary>
    /// Returns client by ID
    /// </summary>
    public ClientDto? Read(int id)
    {
        var client = _testData.Customers.FirstOrDefault(c => c.Id == id);
        if (client == null) return null;

        return new ClientDto
        {
            Id = client.Id,
            DriverLicenseNumber = client.DriverLicenseNumber,
            FullName = client.FullName,
            DateOfBirth = client.DateOfBirth
        };
    }

    /// <summary>
    /// Creates new client
    /// </summary>
    public ClientDto? Create(ClientCreateUpdateDto dto)
    {
        var nextId = _testData.Customers.Any() ? _testData.Customers.Max(c => c.Id) + 1 : 1;

        var client = new Customer
        {
            Id = nextId,
            DriverLicenseNumber = dto.DriverLicenseNumber,
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth
        };

        _testData.Customers.Add(client);

        return new ClientDto
        {
            Id = client.Id,
            DriverLicenseNumber = client.DriverLicenseNumber,
            FullName = client.FullName,
            DateOfBirth = client.DateOfBirth
        };
    }

    /// <summary>
    /// Updates existing client
    /// </summary>
    public bool Update(ClientCreateUpdateDto dto, int id)
    {
        var client = _testData.Customers.FirstOrDefault(c => c.Id == id);
        if (client == null) return false;

        client.DriverLicenseNumber = dto.DriverLicenseNumber;
        client.FullName = dto.FullName;
        client.DateOfBirth = dto.DateOfBirth;

        return true;
    }

    /// <summary>
    /// Deletes client by ID
    /// </summary>
    public bool Delete(int id)
    {
        var client = _testData.Customers.FirstOrDefault(c => c.Id == id);
        if (client == null) return false;

        return _testData.Customers.Remove(client);
    }
}
