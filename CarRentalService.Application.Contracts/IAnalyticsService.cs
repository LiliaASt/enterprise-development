using CarRentalService.Application.Contracts.Analytics;

namespace CarRentalService.Application.Contracts;

/// <summary>
/// Analytics service interface
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Get customers who rented cars of specific model name
    /// </summary>
    public Task<List<string>> ReadCustomersByModelName(string modelName);

    /// <summary>
    /// Get customers who rented cars of specific model ID
    /// </summary>
    public Task<List<string>> ReadCustomersByModelId(int modelId);

    /// <summary>
    /// Get currently rented cars
    /// </summary>
    public Task<List<CarRentalResponse>> ReadCarsInRent(DateTime atTime);

    /// <summary>
    /// Get top N most rented cars
    /// </summary>
    public Task<List<TopCarResponse>> ReadTopMostRentedCars(int count = 5);

    /// <summary>
    /// Get rental count for all cars
    /// </summary>
    public Task<List<CarRentalCountResponse>> ReadAllCarsWithRentalCount();

    /// <summary>
    /// Get top N customers by total rental revenue
    /// </summary>
    public Task<List<TopCustomerResponse>> ReadTopCustomersByTotalAmount(int count = 5);
}
