using CarRentalService.API.DTOs.Requests;
using CarRentalService.API.DTOs.Responses;
using CarRentalService.API.Interfaces;
using CarRentalService.Domain.Data;
using CarRentalService.Domain.Models;
using Mapster;

namespace CarRentalService.API.Services;

/// <summary>
/// Car service implementation
/// </summary>
public class CarService : IApplicationService<CarDto, CarCreateUpdateDto>
{
    private readonly List<Car> _cars;
    private readonly TestData _testData;
    private int _nextId;

    public CarService(TestData testData)
    {
        _testData = testData;
        _cars = testData.Cars.ToList();
        _nextId = _cars.Any() ? _cars.Max(c => c.Id) + 1 : 1;
    }

    public List<CarDto> ReadAll()
    {
        return _cars.Adapt<List<CarDto>>();
    }

    public CarDto? Read(int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        return car?.Adapt<CarDto>();
    }

    public CarDto? Create(CarCreateUpdateDto dto)
    {
        var carModelGeneration = _testData.CarModelGenerations
            .FirstOrDefault(g => g.Id == dto.CarModelGenerationId);

        if (carModelGeneration == null)
            return null;

        var car = new Car
        {
            Id = _nextId++,
            LicensePlate = dto.LicensePlate,
            Color = dto.Color,
            CarModelGeneration = carModelGeneration
        };

        _cars.Add(car);
        return car.Adapt<CarDto>();
    }

    public bool Update(CarCreateUpdateDto dto, int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return false;

        var carModelGeneration = _testData.CarModelGenerations
            .FirstOrDefault(g => g.Id == dto.CarModelGenerationId);

        if (carModelGeneration == null) return false;

        car.LicensePlate = dto.LicensePlate;
        car.Color = dto.Color;
        car.CarModelGeneration = carModelGeneration;

        return true;
    }

    public bool Delete(int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return false;

        return _cars.Remove(car);
    }
}
