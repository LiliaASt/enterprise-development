using AutoMapper;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application.Services;

/// <summary>
/// Service implementation for managing rent transactions
/// </summary>
/// <param name="rentRepository">Rent repository</param>
/// <param name="carRepository">Car repository</param>
/// <param name="customerRepository">Customer repository</param>
/// <param name="generationRepository">Car model generation repository</param>
/// <param name="mapper">AutoMapper instance</param>
public class RentService(
    IRepository<Rent, int> rentRepository,
    IRepository<Car, int> carRepository,
    IRepository<Customer, int> customerRepository,
    IRepository<CarModelGeneration, int> generationRepository,
    IRepository<CarModel, int> carModelRepository,
    IMapper mapper
) : IRentService
{
    /// <summary>
    /// Creates a new rent transaction
    /// </summary>
    /// <param name="dto">Rent creation data</param>
    /// <returns>Created rent DTO</returns>
    public async Task<RentDto> Create(RentCreateUpdateDto dto)
    {
        var allRents = await rentRepository.ReadAll();
        var maxId = allRents.Any() ? allRents.Max(r => r.Id) : 0;

        var car = await carRepository.Read(dto.CarId);
        if (car == null)
            throw new ArgumentException($"Car with id {dto.CarId} not found");

        var customer = await customerRepository.Read(dto.ClientId);
        if (customer == null)
            throw new ArgumentException($"Client with id {dto.ClientId} not found");

        var rent = mapper.Map<Rent>(dto);
        rent.Id = maxId + 1;
        rent.CarId = car.Id;
        rent.CustomerId = customer.Id;

        var created = await rentRepository.Create(rent);
        return await MapToDtoWithDetails(created);
    }

    /// <summary>
    /// Deletes a rent transaction by its identifier
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
        => await rentRepository.Delete(id);

    /// <summary>
    /// Retrieves a rent transaction by its identifier
    /// </summary>
    /// <param name="id">Rent identifier</param>
    /// <returns>Rent DTO if found, null otherwise</returns>
    public async Task<RentDto?> Get(int id)
    {
        var rent = await rentRepository.Read(id);
        return rent != null ? await MapToDtoWithDetails(rent) : null;
    }

    /// <summary>
    /// Retrieves all rent transactions
    /// </summary>
    /// <returns>List of all rent DTOs</returns>
    public async Task<IList<RentDto>> GetAll()
    {
        var rents = await rentRepository.ReadAll();
        var result = new List<RentDto>();

        foreach (var rent in rents)
        {
            result.Add(await MapToDtoWithDetails(rent));
        }

        return result;
    }

    /// <summary>
    /// Updates an existing rent transaction
    /// </summary>
    /// <param name="dto">Rent update data</param>
    /// <param name="id">Rent identifier</param>
    /// <returns>Updated rent DTO</returns>
    public async Task<RentDto> Update(RentCreateUpdateDto dto, int id)
    {
        var rent = await rentRepository.Read(id);
        if (rent == null)
            throw new KeyNotFoundException($"Rent with id {id} not found");

        var car = await carRepository.Read(dto.CarId);
        if (car == null)
            throw new ArgumentException($"Car with id {dto.CarId} not found");

        var customer = await customerRepository.Read(dto.ClientId);
        if (customer == null)
            throw new ArgumentException($"Client with id {dto.ClientId} not found");

        mapper.Map(dto, rent);
        rent.CarId = car.Id;
        rent.CustomerId = customer.Id;

        var updated = await rentRepository.Update(rent);
        return await MapToDtoWithDetails(updated);
    }

    /// <summary>
    /// Retrieves all rental transactions for a specific client
    /// </summary>
    /// <param name="clientId">Client identifier</param>
    /// <returns>List of rent DTOs for the specified client</returns>
    public async Task<IList<RentDto>> GetRentalsByClientAsync(int clientId)
    {
        var rents = await rentRepository.ReadAll();
        var clientRents = rents.Where(r => r.CustomerId == clientId).ToList();

        var result = new List<RentDto>();
        foreach (var rent in clientRents)
        {
            result.Add(await MapToDtoWithDetails(rent));
        }

        return result;
    }

    /// <summary>
    /// Retrieves all rental transactions for a specific car
    /// </summary>
    /// <param name="carId">Car identifier</param>
    /// <returns>List of rent DTOs for the specified car</returns>
    public async Task<IList<RentDto>> GetRentalsByCarAsync(int carId)
    {
        var rents = await rentRepository.ReadAll();
        var carRents = rents.Where(r => r.CarId == carId).ToList();

        var result = new List<RentDto>();
        foreach (var rent in carRents)
        {
            result.Add(await MapToDtoWithDetails(rent));
        }

        return result;
    }

    /// <summary>
    /// Maps Rent entity to DTO with additional details
    /// </summary>
    /// <param name="rent">Rent entity</param>
    /// <returns>Rent DTO with details</returns>
    private async Task<RentDto> MapToDtoWithDetails(Rent rent)
    {
        var car = await carRepository.Read(rent.CarId);
        var customer = await customerRepository.Read(rent.CustomerId);

        if (car == null || customer == null)
            throw new InvalidOperationException("Car or Customer not found for rent");

        var generation = await generationRepository.Read(car.CarModelGenerationId);
        if (generation == null)
            throw new InvalidOperationException($"CarModelGeneration {car.CarModelGenerationId} not found");

        var carModel = await carModelRepository.Read(generation.CarModelId);

        return new RentDto
        {
            Id = rent.Id,
            Car = new Contracts.Shared.CarSimpleDto
            {
                Id = car.Id,
                LicensePlate = car.LicensePlate,
                ModelName = carModel?.Name ?? "Unknown",
                RentalCostPerHour = generation.RentalCostPerHour
            },
            Client = new Contracts.Shared.ClientSimpleDto
            {
                Id = customer.Id,
                FullName = customer.FullName
            },
            StartTime = rent.StartTime,
            Duration = rent.Duration
        };
    }
}
