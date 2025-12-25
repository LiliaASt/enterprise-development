using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.Shared;
using CarRentalService.Domain.Models;
using CarRentalService.Domain.TestData;

namespace CarRentalService.Application.Services;

/// <summary>
/// Rent service implementation
/// </summary>
/// <param name="testData">Test data provider for in-memory storage</param>
public class RentService(TestData testData) : IRentService
{
    private readonly TestData _testData = testData;

    /// <summary>
    /// Returns all rents
    /// </summary>
    public List<RentDto> ReadAll()
    {
        return _testData.Rents.Select(r => new RentDto
        {
            Id = r.Id,
            Car = new CarSimpleDto
            {
                Id = r.Car.Id,
                LicensePlate = r.Car.LicensePlate,
                ModelName = r.Car.CarModelGeneration.CarModel.Name,
                RentalCostPerHour = r.Car.CarModelGeneration.RentalCostPerHour
            },
            Client = new ClientSimpleDto
            {
                Id = r.Customer.Id,
                FullName = r.Customer.FullName
            },
            StartTime = r.StartTime,
            Duration = r.Duration
        }).ToList();
    }

    /// <summary>
    /// Returns rent by ID
    /// </summary>
    public RentDto? Read(int id)
    {
        var rent = _testData.Rents.FirstOrDefault(r => r.Id == id);
        if (rent == null) return null;

        return new RentDto
        {
            Id = rent.Id,
            Car = new CarSimpleDto
            {
                Id = rent.Car.Id,
                LicensePlate = rent.Car.LicensePlate,
                ModelName = rent.Car.CarModelGeneration.CarModel.Name,
                RentalCostPerHour = rent.Car.CarModelGeneration.RentalCostPerHour
            },
            Client = new ClientSimpleDto
            {
                Id = rent.Customer.Id,
                FullName = rent.Customer.FullName
            },
            StartTime = rent.StartTime,
            Duration = rent.Duration
        };
    }

    /// <summary>
    /// Creates new rent
    /// </summary>
    public RentDto? Create(RentCreateUpdateDto dto)
    {
        var car = _testData.Cars.FirstOrDefault(c => c.Id == dto.CarId);
        var client = _testData.Customers.FirstOrDefault(c => c.Id == dto.ClientId);

        if (car == null || client == null)
            return null;

        var nextId = _testData.Rents.Any() ? _testData.Rents.Max(r => r.Id) + 1 : 1;

        var rent = new Rent
        {
            Id = nextId,
            Car = car,
            Customer = client,
            StartTime = dto.StartTime,
            Duration = dto.Duration
        };

        _testData.Rents.Add(rent);

        return new RentDto
        {
            Id = rent.Id,
            Car = new CarSimpleDto
            {
                Id = car.Id,
                LicensePlate = car.LicensePlate,
                ModelName = car.CarModelGeneration.CarModel.Name,
                RentalCostPerHour = car.CarModelGeneration.RentalCostPerHour
            },
            Client = new ClientSimpleDto
            {
                Id = client.Id,
                FullName = client.FullName
            },
            StartTime = rent.StartTime,
            Duration = rent.Duration
        };
    }

    /// <summary>
    /// Updates existing rent
    /// </summary>
    public bool Update(RentCreateUpdateDto dto, int id)
    {
        var rent = _testData.Rents.FirstOrDefault(r => r.Id == id);
        if (rent == null) return false;

        var car = _testData.Cars.FirstOrDefault(c => c.Id == dto.CarId);
        var client = _testData.Customers.FirstOrDefault(c => c.Id == dto.ClientId);

        if (car == null || client == null) return false;

        rent.Car = car;
        rent.Customer = client;
        rent.StartTime = dto.StartTime;
        rent.Duration = dto.Duration;

        return true;
    }

    /// <summary>
    /// Deletes rent by ID
    /// </summary>
    public bool Delete(int id)
    {
        var rent = _testData.Rents.FirstOrDefault(r => r.Id == id);
        if (rent == null) return false;

        return _testData.Rents.Remove(rent);
    }
}
