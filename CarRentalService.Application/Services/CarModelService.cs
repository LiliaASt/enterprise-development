using AutoMapper;
using CarRentalService.Application.Contracts.CarModel;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application.Services;

/// <summary>
/// Service implementation for managing car models
/// </summary>
/// <param name="carModelRepository">Car model repository</param>
/// <param name="carRepository">Car repository</param>
/// <param name="generationRepository">Car model generation repository</param>
/// <param name="mapper">AutoMapper instance</param>
public class CarModelService(
    IRepository<CarModel, int> carModelRepository,
    IRepository<Car, int> carRepository,
    IMapper mapper
) : ICarModelService
{
    /// <summary>
    /// Creates a new car model
    /// </summary>
    /// <param name="dto">Car model creation data</param>
    /// <returns>Created car model DTO</returns>
    public async Task<CarModelDto> Create(CarModelCreateUpdateDto dto)
    {
        var allModels = await carModelRepository.ReadAll();
        var maxId = allModels.Any() ? allModels.Max(m => m.Id) : 0;

        var carModel = mapper.Map<CarModel>(dto);
        carModel.Id = maxId + 1;

        var created = await carModelRepository.Create(carModel);
        return mapper.Map<CarModelDto>(created);
    }

    /// <summary>
    /// Deletes a car model by its identifier
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
        => await carModelRepository.Delete(id);

    /// <summary>
    /// Retrieves a car model by its identifier
    /// </summary>
    /// <param name="id">Car model identifier</param>
    /// <returns>Car model DTO if found, null otherwise</returns>
    public async Task<CarModelDto?> Get(int id)
    {
        var carModel = await carModelRepository.Read(id);
        return carModel != null ? mapper.Map<CarModelDto>(carModel) : null;
    }

    /// <summary>
    /// Retrieves all car models
    /// </summary>
    /// <returns>List of all car model DTOs</returns>
    public async Task<IList<CarModelDto>> GetAll()
    {
        var carModels = await carModelRepository.ReadAll();
        return mapper.Map<List<CarModelDto>>(carModels);
    }

    /// <summary>
    /// Updates an existing car model
    /// </summary>
    /// <param name="dto">Car model update data</param>
    /// <param name="id">Car model identifier</param>
    /// <returns>Updated car model DTO</returns>
    public async Task<CarModelDto> Update(CarModelCreateUpdateDto dto, int id)
    {
        var carModel = await carModelRepository.Read(id);
        if (carModel == null)
            throw new KeyNotFoundException($"CarModel with id {id} not found");

        mapper.Map(dto, carModel);
        var updated = await carModelRepository.Update(carModel);
        return mapper.Map<CarModelDto>(updated);
    }

    /// <summary>
    /// Retrieves all cars associated with a specific car model
    /// </summary>
    /// <param name="modelId">Car model identifier</param>
    /// <returns>List of cars belonging to the specified model</returns>
    public async Task<IList<CarDto>> GetCarsByModelAsync(int modelId)
    {
        var cars = await carRepository.ReadAll();

        var carsForModel = cars
            .Where(c => c.CarModelGeneration.CarModel.Id == modelId)
            .ToList();

        return mapper.Map<List<CarDto>>(carsForModel);
    }
}
