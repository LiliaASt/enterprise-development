using CarRentalService.API.DTOs.Requests;
using CarRentalService.API.DTOs.Responses;
using CarRentalService.API.Interfaces;
using CarRentalService.Domain.Data;
using CarRentalService.Domain.Models;
using Mapster;

namespace CarRentalService.API.Services;

/// <summary>
/// Rent service implementation
/// </summary>
public class RentService : IApplicationService<RentDto, RentCreateUpdateDto>
{
    private readonly List<Rent> _rents;
    private readonly List<Car> _cars;
    private readonly List<Customer> _customers;
    private int _nextId;

    public RentService(TestData testData)
    {
        _rents = testData.Rents.ToList();
        _cars = testData.Cars.ToList();
        _customers = testData.Customers.ToList();
        _nextId = _rents.Any() ? _rents.Max(r => r.Id) + 1 : 1;
    }

    public List<RentDto> ReadAll()
    {
        return _rents.Adapt<List<RentDto>>();
    }

    public RentDto? Read(int id)
    {
        var rent = _rents.FirstOrDefault(r => r.Id == id);
        return rent?.Adapt<RentDto>();
    }

    public RentDto? Create(RentCreateUpdateDto dto)
    {
        var car = _cars.FirstOrDefault(c => c.Id == dto.CarId);
        var client = _customers.FirstOrDefault(c => c.Id == dto.ClientId);

        if (car == null || client == null)
            return null;

        var rent = new Rent
        {
            Id = _nextId++,
            Car = car,
            Customer = client,
            StartTime = dto.StartTime,
            Duration = dto.Duration
        };

        _rents.Add(rent);
        return rent.Adapt<RentDto>();
    }

    public bool Update(RentCreateUpdateDto dto, int id)
    {
        var rent = _rents.FirstOrDefault(r => r.Id == id);
        if (rent == null) return false;

        var car = _cars.FirstOrDefault(c => c.Id == dto.CarId);
        var client = _customers.FirstOrDefault(c => c.Id == dto.ClientId);

        if (car == null || client == null) return false;

        rent.Car = car;
        rent.Customer = client;
        rent.StartTime = dto.StartTime;
        rent.Duration = dto.Duration;

        return true;
    }

    public bool Delete(int id)
    {
        var rent = _rents.FirstOrDefault(r => r.Id == id);
        if (rent == null) return false;

        return _rents.Remove(rent);
    }
}
