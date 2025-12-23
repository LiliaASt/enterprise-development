using CarRentalService.API.DTOs.Responses;

namespace CarRentalService.API.Interfaces;

/// <summary>
/// Analytics service interface
/// Based on tests from your first lab work
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Get customers who rented cars of specific model (from test: GetCustomersByCarModel_ShouldReturnCustomersOrderedByFullName)
    /// </summary>
    List<string> ReadCustomersByModelName(string modelName);

    /// <summary>
    /// Get customers who rented cars of specific model ID
    /// </summary>
    List<string> ReadCustomersByModelId(int modelId);

    /// <summary>
    /// Get currently rented cars (from test: GetCarsCurrentlyRented_ShouldReturnActiveRentals)
    /// </summary>
    List<CarRentalResponse> ReadCarsInRent(DateTime atTime);

    /// <summary>
    /// Get top N most rented cars (from test: GetTop5MostFrequentlyRentedCars_ShouldReturnExpectedCars)
    /// </summary>
    List<TopCarResponse> ReadTopMostRentedCars(int count = 5);

    /// <summary>
    /// Get rental count for each car (from test: GetRentalCountPerCar_ShouldReturnCountForEachCar)
    /// </summary>
    List<CarRentalCountResponse> ReadAllCarsWithRentalCount();

    /// <summary>
    /// Get top N customers by total rental amount (from test: GetTop5CustomersByRentalSum_ShouldReturnCorrectOrder)
    /// </summary>
    List<TopCustomerResponse> ReadTopCustomersByTotalAmount(int count = 5);

    /// <summary>
    /// Additional method: Get customers by car model name (from test: GetCustomersByCarModelName_ShouldReturnCorrectCustomers)
    /// </summary>
    List<string> ReadCustomersByCarModelName(string modelName);
}
