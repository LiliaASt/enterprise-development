using AutoMapper;
using CarRentalService.Application.Contracts.CarModelGeneration;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application.Services;

/// <summary>
/// Service implementation for managing car model generations
/// </summary>
/// <param name="generationRepository">Car model generation repository</param>
/// <param name="carModelRepository">Car model repository</param>
/// <param name="mapper">AutoMapper instance</param>
public class CarModelGenerationService(
    IRepository<CarModelGeneration, int> generationRepository,
    IRepository<CarModel, int> carModelRepository,
    IMapper mapper
) : ICarModelGenerationService
{
    /// <summary>
    /// Creates a new car model generation
    /// </summary>
    /// <param name="dto">Car model generation creation data</param>
    /// <returns>Created car model generation DTO</returns>
    public async Task<CarModelGenerationDto> Create(CarModelGenerationCreateUpdateDto dto)
    {
        var allGenerations = await generationRepository.ReadAll();
        var maxId = allGenerations.Any() ? allGenerations.Max(g => g.Id) : 0;

        var carModel = await carModelRepository.Read(dto.CarModelId);
        if (carModel == null)
            throw new ArgumentException($"CarModel with id {dto.CarModelId} not found");

        var generation = mapper.Map<CarModelGeneration>(dto);
        generation.Id = maxId + 1;
        generation.CarModelId = carModel.Id;

        var created = await generationRepository.Create(generation);
        return await MapToDtoWithDetails(created);
    }

    /// <summary>
    /// Deletes a car model generation by its identifier
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
        => await generationRepository.Delete(id);

    /// <summary>
    /// Retrieves a car model generation by its identifier
    /// </summary>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>Car model generation DTO if found, null otherwise</returns>
    public async Task<CarModelGenerationDto?> Get(int id)
    {
        var generation = await generationRepository.Read(id);
        return generation != null ? await MapToDtoWithDetails(generation) : null;
    }

    /// <summary>
    /// Retrieves all car model generations
    /// </summary>
    /// <returns>List of all car model generation DTOs</returns>
    public async Task<IList<CarModelGenerationDto>> GetAll()
    {
        var generations = await generationRepository.ReadAll();
        var carModels = await carModelRepository.ReadAll();

        var result = new List<CarModelGenerationDto>();
        foreach (var generation in generations)
        {
            var carModel = carModels.FirstOrDefault(m => m.Id == generation.CarModelId);
            result.Add(new CarModelGenerationDto
            {
                Id = generation.Id,
                CarModelId = generation.CarModelId,
                CarModelName = carModel?.Name ?? "Unknown",
                ProductionYear = generation.ProductionYear,
                EngineVolume = generation.EngineVolume,
                TransmissionType = generation.TransmissionType,
                RentalCostPerHour = generation.RentalCostPerHour
            });
        }

        return result;
    }

    /// <summary>
    /// Updates an existing car model generation
    /// </summary>
    /// <param name="dto">Car model generation update data</param>
    /// <param name="id">Car model generation identifier</param>
    /// <returns>Updated car model generation DTO</returns>
    public async Task<CarModelGenerationDto> Update(CarModelGenerationCreateUpdateDto dto, int id)
    {
        var generation = await generationRepository.Read(id);
        if (generation == null)
            throw new KeyNotFoundException($"CarModelGeneration with id {id} not found");

        var carModel = await carModelRepository.Read(dto.CarModelId);
        if (carModel == null)
            throw new ArgumentException($"CarModel with id {dto.CarModelId} not found");

        mapper.Map(dto, generation);
        generation.CarModelId = carModel.Id;

        var updated = await generationRepository.Update(generation);
        return await MapToDtoWithDetails(updated);
    }

    /// <summary>
    /// Maps CarModelGeneration entity to DTO with additional details
    /// </summary>
    /// <param name="generation">Car model generation entity</param>
    /// <returns>Car model generation DTO with details</returns>
    private async Task<CarModelGenerationDto> MapToDtoWithDetails(CarModelGeneration generation)
    {
        var carModel = await carModelRepository.Read(generation.CarModelId);

        return new CarModelGenerationDto
        {
            Id = generation.Id,
            CarModelId = generation.CarModelId,
            CarModelName = carModel?.Name ?? "Unknown",
            ProductionYear = generation.ProductionYear,
            EngineVolume = generation.EngineVolume,
            TransmissionType = generation.TransmissionType,
            RentalCostPerHour = generation.RentalCostPerHour
        };
    }
}
