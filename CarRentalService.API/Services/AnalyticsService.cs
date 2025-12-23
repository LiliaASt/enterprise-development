using CarRentalService.API.DTOs.Responses;
using CarRentalService.API.Interfaces;
using CarRentalService.Domain.Data;

namespace CarRentalService.API.Services;

/// <summary>
/// Analytics service implementation based on unit tests from first lab
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly TestData _testData;

    public AnalyticsService(TestData testData)
    {
        _testData = testData;
    }

    // Реализация теста: GetCustomersByCarModel_ShouldReturnCustomersOrderedByFullName
    public List<string> ReadCustomersByModelName(string modelName)
    {
        return _testData.Rents
            .Where(r => r.Car.CarModelGeneration.CarModel.Name == modelName)
            .Select(r => r.Customer.FullName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();
    }

    public List<string> ReadCustomersByModelId(int modelId)
    {
        return _testData.Rents
            .Where(r => r.Car.CarModelGeneration.CarModel.Id == modelId)
            .Select(r => r.Customer.FullName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();
    }

    // Реализация теста: GetCarsCurrentlyRented_ShouldReturnActiveRentals
    public List<CarRentalResponse> ReadCarsInRent(DateTime atTime)
    {
        return _testData.Rents
            .Where(r =>
            {
                var rentalEnd = r.StartTime.AddHours(r.Duration);
                return r.StartTime <= atTime && atTime <= rentalEnd;
            })
            .Select(r => new CarRentalResponse
            {
                CarId = r.Car.Id,
                LicensePlate = r.Car.LicensePlate,
                Color = r.Car.Color,
                ModelName = r.Car.CarModelGeneration.CarModel.Name,
                ClientName = r.Customer.FullName,
                RentalStart = r.StartTime,
                RentalEnd = r.StartTime.AddHours(r.Duration)
            })
            .DistinctBy(r => r.CarId)
            .OrderBy(r => r.RentalStart)
            .ToList();
    }

    // Реализация теста: GetTop5MostFrequentlyRentedCars_ShouldReturnExpectedCars
    public List<TopCarResponse> ReadTopMostRentedCars(int count = 5)
    {
        return _testData.Rents
            .GroupBy(r => r.Car)
            .Select(g => new TopCarResponse
            {
                CarId = g.Key.Id,
                LicensePlate = g.Key.LicensePlate,
                ModelName = g.Key.CarModelGeneration.CarModel.Name,
                RentalCount = g.Count(),
                TotalHours = g.Sum(r => r.Duration),
                TotalRevenue = (decimal)g.Sum(r => r.Duration * (double)g.Key.CarModelGeneration.RentalCostPerHour)
            })
            .OrderByDescending(x => x.RentalCount)
            .ThenBy(x => x.LicensePlate)
            .Take(count)
            .ToList();
    }

    // Реализация теста: GetRentalCountPerCar_ShouldReturnCountForEachCar
    public List<CarRentalCountResponse> ReadAllCarsWithRentalCount()
    {
        var allCars = _testData.Cars.ToList();
        var rentsByCar = _testData.Rents
            .GroupBy(r => r.Car.Id)
            .ToDictionary(g => g.Key, g => g.Count());

        return allCars.Select(car => new CarRentalCountResponse
        {
            CarId = car.Id,
            LicensePlate = car.LicensePlate,
            ModelName = car.CarModelGeneration.CarModel.Name,
            RentalCount = rentsByCar.GetValueOrDefault(car.Id, 0)
        })
        .OrderByDescending(x => x.RentalCount)
        .ThenBy(x => x.CarId)
        .ToList();
    }

    // Реализация теста: GetTop5CustomersByRentalSum_ShouldReturnCorrectOrder
    public List<TopCustomerResponse> ReadTopCustomersByTotalAmount(int count = 5)
    {
        return _testData.Rents
            .GroupBy(r => r.Customer)
            .Select(g => new TopCustomerResponse
            {
                CustomerId = g.Key.Id,
                FullName = g.Key.FullName,
                RentalCount = g.Count(),
                TotalRevenue = (decimal)g.Sum(r => r.Duration * (double)r.Car.CarModelGeneration.RentalCostPerHour)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ThenBy(x => x.FullName)
            .Take(count)
            .ToList();
    }

    // Дополнительный метод из теста: GetCustomersByCarModelName_ShouldReturnCorrectCustomers
    public List<string> ReadCustomersByCarModelName(string modelName)
    {
        return ReadCustomersByModelName(modelName);
    }
}
