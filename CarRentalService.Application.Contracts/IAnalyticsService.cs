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
    public List<string> ReadCustomersByModelName(string modelName);

    /// <summary>
    /// Get customers who rented cars of specific model ID
    /// </summary>
    public List<string> ReadCustomersByModelId(int modelId);

    /// <summary>
    /// Get currently rented cars
    /// </summary>
    public List<CarRentalResponse> ReadCarsInRent(DateTime atTime);

    /// <summary>
    /// Get top N most rented cars
    /// </summary>
    public List<TopCarResponse> ReadTopMostRentedCars(int count = 5);

    /// <summary>
    /// Get rental count for each car
    /// </summary>
    public List<CarRentalCountResponse> ReadAllCarsWithRentalCount();

    /// <summary>
    /// Get top N customers by total rental amount
    /// </summary>
    public List<TopCustomerResponse> ReadTopCustomersByTotalAmount(int count = 5);
}
