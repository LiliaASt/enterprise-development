using CarRentalService.Application.Contracts;
using CarRentalService.Application.Contracts.Analytics;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application.Services;

/// <summary>
/// Analytics service implementation for car rental business intelligence
/// </summary>
/// <param name="rentRepository">Rent repository</param>
/// <param name="carRepository">Car repository</param>
/// <param name="customerRepository">Customer repository</param>
/// <param name="carModelRepository">Car model repository</param>
/// <param name="generationRepository">Car model generation repository</param>
public class AnalyticsService(
    IRepository<Rent, int> rentRepository,
    IRepository<Car, int> carRepository,
    IRepository<Customer, int> customerRepository,
    IRepository<CarModel, int> carModelRepository,
    IRepository<CarModelGeneration, int> generationRepository
) : IAnalyticsService
{
    /// <summary>
    /// Get customers who rented cars of specific model name
    /// </summary>
    /// <param name="modelName">Car model name to filter by</param>
    /// <returns>List of customer full names ordered alphabetically</returns>
    public async Task<List<string>> ReadCustomersByModelName(string modelName)
    {
        var allRents = await rentRepository.ReadAll();
        var allCustomers = await customerRepository.ReadAll();
        var allCars = await carRepository.ReadAll();
        var allGenerations = await generationRepository.ReadAll();
        var allCarModels = await carModelRepository.ReadAll();

        var targetModel = allCarModels.FirstOrDefault(m =>
            m.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase));

        if (targetModel == null)
            return new List<string>();

        var modelGenerationIds = allGenerations
            .Where(g => g.CarModelId == targetModel.Id)
            .Select(g => g.Id)
            .ToList();

        var carIds = allCars
            .Where(c => modelGenerationIds.Contains(c.CarModelGenerationId))
            .Select(c => c.Id)
            .ToList();

        var customerIds = allRents
            .Where(r => carIds.Contains(r.CarId))
            .Select(r => r.CustomerId)
            .Distinct()
            .ToList();

        return allCustomers
            .Where(c => customerIds.Contains(c.Id))
            .Select(c => c.FullName)
            .OrderBy(name => name)
            .ToList();
    }

    /// <summary>
    /// Get customers who rented cars of specific model ID
    /// </summary>
    /// /// <param name="modelId">Car model ID to filter by</param>
    /// <returns>List of customer full names ordered alphabetically</returns>
    public async Task<List<string>> ReadCustomersByModelId(int modelId)
    {
        var allRents = await rentRepository.ReadAll();
        var allCustomers = await customerRepository.ReadAll();
        var allCars = await carRepository.ReadAll();
        var allGenerations = await generationRepository.ReadAll();

        var modelGenerationIds = allGenerations
            .Where(g => g.CarModelId == modelId)
            .Select(g => g.Id)
            .ToList();

        var carIds = allCars
            .Where(c => modelGenerationIds.Contains(c.CarModelGenerationId))
            .Select(c => c.Id)
            .ToList();

        var customerIds = allRents
            .Where(r => carIds.Contains(r.CarId))
            .Select(r => r.CustomerId)
            .Distinct()
            .ToList();

        return allCustomers
            .Where(c => customerIds.Contains(c.Id))
            .Select(c => c.FullName)
            .OrderBy(name => name)
            .ToList();
    }

    /// <summary>
    /// Get currently rented cars at specific time
    /// </summary>
    /// <param name="atTime">Time to check for active rentals</param>
    /// <returns>List of currently rented cars with rental details</returns>
    public async Task<List<CarRentalResponse>> ReadCarsInRent(DateTime atTime)
    {
        var allRents = await rentRepository.ReadAll();
        var allCars = await carRepository.ReadAll();
        var allCustomers = await customerRepository.ReadAll();
        var allGenerations = await generationRepository.ReadAll();
        var allCarModels = await carModelRepository.ReadAll();

        var activeRentals = allRents
            .Where(r =>
            {
                var rentalEnd = r.StartTime.AddHours(r.Duration);
                return r.StartTime <= atTime && atTime <= rentalEnd;
            })
            .ToList();

        var result = new List<CarRentalResponse>();

        foreach (var rental in activeRentals)
        {
            var car = allCars.FirstOrDefault(c => c.Id == rental.CarId);
            var customer = allCustomers.FirstOrDefault(c => c.Id == rental.CustomerId);
            var generation = allGenerations.FirstOrDefault(g => g.Id == car?.CarModelGenerationId);
            var carModel = allCarModels.FirstOrDefault(m => m.Id == generation?.CarModelId);

            if (car != null && customer != null && generation != null)
            {
                result.Add(new CarRentalResponse
                {
                    CarId = car.Id,
                    LicensePlate = car.LicensePlate,
                    Color = car.Color,
                    ModelName = carModel?.Name ?? "Unknown",
                    ClientName = customer.FullName,
                    RentalStart = rental.StartTime,
                    RentalEnd = rental.StartTime.AddHours(rental.Duration)
                });
            }
        }

        return result
            .DistinctBy(r => r.CarId)
            .OrderBy(r => r.RentalStart)
            .ToList();
    }

    /// <summary>
    /// Get top N most rented cars
    /// </summary>
    /// <param name="count">Number of top cars to return (default: 5)</param>
    /// <returns>List of top rented cars with rental statistics</returns>
    public async Task<List<TopCarResponse>> ReadTopMostRentedCars(int count = 5)
    {
        var allRents = await rentRepository.ReadAll();
        var allCars = await carRepository.ReadAll();
        var allGenerations = await generationRepository.ReadAll();
        var allCarModels = await carModelRepository.ReadAll();

        var carGroups = allRents
            .GroupBy(r => r.CarId)
            .Select(g => new
            {
                CarId = g.Key,
                RentalCount = g.Count(),
                TotalHours = g.Sum(r => r.Duration)
            });

        var carRentalStats = new List<TopCarResponse>();

        foreach (var group in carGroups)
        {
            var car = allCars.FirstOrDefault(c => c.Id == group.CarId);
            if (car == null) continue;

            var generation = allGenerations.FirstOrDefault(g => g.Id == car.CarModelGenerationId);
            if (generation == null) continue;

            var carModel = allCarModels.FirstOrDefault(m => m.Id == generation.CarModelId);

            carRentalStats.Add(new TopCarResponse
            {
                CarId = car.Id,
                LicensePlate = car.LicensePlate,
                ModelName = carModel?.Name ?? "Unknown",
                RentalCount = group.RentalCount,
                TotalHours = group.TotalHours,
                TotalRevenue = (decimal)(group.TotalHours * (double)generation.RentalCostPerHour)
            });
        }

        return carRentalStats
            .OrderByDescending(x => x.RentalCount)
            .ThenBy(x => x.LicensePlate)
            .Take(count)
            .ToList();
    }

    /// <summary>
    /// Get rental count for all cars
    /// </summary>
    /// <returns>List of all cars with their rental counts</returns>
    public async Task<List<CarRentalCountResponse>> ReadAllCarsWithRentalCount()
    {
        var allRents = await rentRepository.ReadAll();
        var allCars = await carRepository.ReadAll();
        var allGenerations = await generationRepository.ReadAll();
        var allCarModels = await carModelRepository.ReadAll();

        var rentsByCar = allRents
            .GroupBy(r => r.CarId)
            .ToDictionary(g => g.Key, g => g.Count());

        return allCars
            .Select(car =>
            {
                var generation = allGenerations.FirstOrDefault(g => g.Id == car.CarModelGenerationId);
                var carModel = generation != null
                    ? allCarModels.FirstOrDefault(m => m.Id == generation.CarModelId)
                    : null;

                return new CarRentalCountResponse
                {
                    CarId = car.Id,
                    LicensePlate = car.LicensePlate,
                    ModelName = carModel?.Name ?? "Unknown",
                    RentalCount = rentsByCar.GetValueOrDefault(car.Id, 0)
                };
            })
            .OrderByDescending(x => x.RentalCount)
            .ThenBy(x => x.CarId)
            .ToList();
    }

    /// <summary>
    /// Get top N customers by total rental revenue
    /// </summary>
    /// <param name="count">Number of top customers to return (default: 5)</param>
    /// <returns>List of top customers by total rental revenue</returns>
    public async Task<List<TopCustomerResponse>> ReadTopCustomersByTotalAmount(int count = 5)
    {
        var allRents = await rentRepository.ReadAll();
        var allCustomers = await customerRepository.ReadAll();
        var allCars = await carRepository.ReadAll();
        var allGenerations = await generationRepository.ReadAll();

        return allRents
            .Select(r => new
            {
                Rental = r,
                Car = allCars.FirstOrDefault(c => c.Id == r.CarId),
                Customer = allCustomers.FirstOrDefault(c => c.Id == r.CustomerId)
            })
            .Where(x => x.Car != null && x.Customer != null)
            .GroupBy(x => x.Customer!.Id)
            .Select(g =>
            {
                var customer = g.First().Customer!;

                var totalRevenue = g.Sum(x =>
                {
                    var generation = allGenerations.FirstOrDefault(gen => gen.Id == x.Car!.CarModelGenerationId);
                    return generation != null
                        ? (decimal)(x.Rental.Duration * (double)generation.RentalCostPerHour)
                        : 0;
                });

                return new TopCustomerResponse
                {
                    CustomerId = customer.Id,
                    FullName = customer.FullName,
                    RentalCount = g.Count(),
                    TotalRevenue = totalRevenue
                };
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ThenBy(x => x.FullName)
            .Take(count)
            .ToList();
    }
}
