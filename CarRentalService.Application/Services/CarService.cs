using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Shared;
using CarRentalService.Domain.Models;
using CarRentalService.Domain.TestData;

namespace CarRentalService.Application.Services;

/// <summary>
/// Car service implementation
/// </summary>
/// <param name="testData">Test data provider for in-memory storage</param>
public class CarService(TestData testData) : ICarService
{
    private readonly TestData _testData = testData;

    /// <summary>
    /// Returns all cars
    /// </summary>
    public List<CarDto> ReadAll()
    {
        return _testData.Cars.Select(car => new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Color = car.Color,
            CarModelGeneration = new CarModelGenerationDto
            {
                Id = car.CarModelGeneration.Id,
                ModelName = car.CarModelGeneration.CarModel.Name,
                ProductionYear = car.CarModelGeneration.ProductionYear,
                RentalCostPerHour = car.CarModelGeneration.RentalCostPerHour
            }
        }).ToList();
    }

    /// <summary>
    /// Returns car by ID
    /// </summary>
    public CarDto? Read(int id)
    {
        var car = _testData.Cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return null;

        return new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Color = car.Color,
            CarModelGeneration = new CarModelGenerationDto
            {
                Id = car.CarModelGeneration.Id,
                ModelName = car.CarModelGeneration.CarModel.Name,
                ProductionYear = car.CarModelGeneration.ProductionYear,
                RentalCostPerHour = car.CarModelGeneration.RentalCostPerHour
            }
        };
    }

    /// <summary>
    /// Creates new car
    /// </summary>
    public CarDto? Create(CarCreateUpdateDto dto)
    {
        var carModelGeneration = _testData.CarModelGenerations
            .FirstOrDefault(g => g.Id == dto.CarModelGenerationId);

        if (carModelGeneration == null)
            return null;

        var nextId = _testData.Cars.Any() ? _testData.Cars.Max(c => c.Id) + 1 : 1;

        var car = new Car
        {
            Id = nextId,
            LicensePlate = dto.LicensePlate,
            Color = dto.Color,
            CarModelGeneration = carModelGeneration
        };

        _testData.Cars.Add(car);

        return new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Color = car.Color,
            CarModelGeneration = new CarModelGenerationDto
            {
                Id = carModelGeneration.Id,
                ModelName = carModelGeneration.CarModel.Name,
                ProductionYear = carModelGeneration.ProductionYear,
                RentalCostPerHour = carModelGeneration.RentalCostPerHour
            }
        };
    }

    /// <summary>
    /// Updates existing car
    /// </summary>
    public bool Update(CarCreateUpdateDto dto, int id)
    {
        var car = _testData.Cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return false;

        var carModelGeneration = _testData.CarModelGenerations
            .FirstOrDefault(g => g.Id == dto.CarModelGenerationId);

        if (carModelGeneration == null) return false;

        car.LicensePlate = dto.LicensePlate;
        car.Color = dto.Color;
        car.CarModelGeneration = carModelGeneration;

        return true;
    }

    /// <summary>
    /// Deletes car by ID
    /// </summary>
    public bool Delete(int id)
    {
        var car = _testData.Cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return false;

        return _testData.Cars.Remove(car);
    }
}
