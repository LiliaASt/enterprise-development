using AutoMapper;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.CarModelGeneration;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application.Services;

/// <summary>
/// Service implementation for managing cars
/// </summary>
/// <param name="carRepository">Car repository</param>
/// <param name="carModelRepository">Car model repository</param>
/// <param name="generationRepository">Car model generation repository</param>
/// <param name="mapper">AutoMapper instance</param>
public class CarService(
    IRepository<Car, int> carRepository,
    IRepository<CarModel, int> carModelRepository,
    IRepository<CarModelGeneration, int> generationRepository,
    IMapper mapper
) : ICarService
{
    /// <summary>
    /// Creates a new car
    /// </summary>
    /// <param name="dto">Car creation data</param>
    /// <returns>Created car DTO</returns>
    public async Task<CarDto> Create(CarCreateUpdateDto dto)
    {
        var allCars = await carRepository.ReadAll();
        var maxId = allCars.Any() ? allCars.Max(c => c.Id) : 0;

        var generation = await generationRepository.Read(dto.CarModelGenerationId);
        if (generation == null)
            throw new ArgumentException($"CarModelGeneration with id {dto.CarModelGenerationId} not found");

        var car = mapper.Map<Car>(dto);
        car.Id = maxId + 1;
        car.CarModelGenerationId = generation.Id;

        var created = await carRepository.Create(car);
        return await MapToDtoWithDetails(created);
    }

    /// <summary>
    /// Deletes a car by its identifier
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
        => await carRepository.Delete(id);

    /// <summary>
    /// Retrieves a car by its identifier
    /// </summary>
    /// <param name="id">Car identifier</param>
    /// <returns>Car DTO if found, null otherwise</returns>
    public async Task<CarDto?> Get(int id)
    {
        var car = await carRepository.Read(id);
        return car != null ? await MapToDtoWithDetails(car) : null;
    }

    /// <summary>
    /// Retrieves all cars
    /// </summary>
    /// <returns>List of all car DTOs</returns>
    public async Task<IList<CarDto>> GetAll()
    {
        var cars = await carRepository.ReadAll();
        var result = new List<CarDto>();

        var allGenerations = await generationRepository.ReadAll();
        var allCarModels = await carModelRepository.ReadAll();

        foreach (var car in cars)
        {
            var generation = allGenerations.FirstOrDefault(g => g.Id == car.CarModelGenerationId);
            if (generation == null) continue;

            var carModel = allCarModels.FirstOrDefault(m => m.Id == generation.CarModelId);

            result.Add(new CarDto
            {
                Id = car.Id,
                LicensePlate = car.LicensePlate,
                Color = car.Color,
                CarModelGeneration = new CarModelGenerationDto
                {
                    Id = generation.Id,
                    CarModelId = generation.CarModelId,
                    CarModelName = carModel?.Name ?? "Unknown",
                    ProductionYear = generation.ProductionYear,
                    EngineVolume = generation.EngineVolume,
                    TransmissionType = generation.TransmissionType,
                    RentalCostPerHour = generation.RentalCostPerHour
                }
            });
        }

        return result;
    }

    /// <summary>
    /// Updates an existing car
    /// </summary>
    /// <param name="dto">Car update data</param>
    /// <param name="id">Car identifier</param>
    /// <returns>Updated car DTO</returns>
    public async Task<CarDto> Update(CarCreateUpdateDto dto, int id)
    {
        var car = await carRepository.Read(id);
        if (car == null)
            throw new KeyNotFoundException($"Car with id {id} not found");

        var generation = await generationRepository.Read(dto.CarModelGenerationId);
        if (generation == null)
            throw new ArgumentException($"CarModelGeneration with id {dto.CarModelGenerationId} not found");

        car.LicensePlate = dto.LicensePlate;
        car.Color = dto.Color;
        car.CarModelGenerationId = generation.Id;

        var updated = await carRepository.Update(car);
        return await MapToDtoWithDetails(updated);
    }

    /// <summary>
    /// Maps Car entity to DTO with additional details
    /// </summary>
    /// <param name="car">Car entity</param>
    /// <returns>Car DTO with details</returns>
    private async Task<CarDto> MapToDtoWithDetails(Car car)
    {
        var generation = await generationRepository.Read(car.CarModelGenerationId);
        if (generation == null)
            throw new InvalidOperationException($"CarModelGeneration {car.CarModelGenerationId} not found");

        var carModel = await carModelRepository.Read(generation.CarModelId);

        return new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Color = car.Color,
            CarModelGeneration = new CarModelGenerationDto
            {
                Id = generation.Id,
                CarModelId = generation.CarModelId,
                CarModelName = carModel?.Name ?? "Unknown",
                ProductionYear = generation.ProductionYear,
                EngineVolume = generation.EngineVolume,
                TransmissionType = generation.TransmissionType,
                RentalCostPerHour = generation.RentalCostPerHour
            }
        };
    }
}
