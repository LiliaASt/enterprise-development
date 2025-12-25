using CarRentalService.Domain.TestData;

namespace CarRentalService.Tests;

/// <summary>
/// Unit tests for the car rental service
/// Tests various business logic scenarios, including customer requests, rental calculations, and data aggregation
/// </summary>
public class CarRentalServiceTests(TestData service) : IClassFixture<TestData>
{
    /// <summary>
    /// Checks whether customers who have rented a specific car model return and whether they make an order using their full name
    /// Checks the correctness of the filtering and sorting logic
    /// </summary>
    [Fact]
    public void GetCustomersByCarModel_ShouldReturnCustomersOrderedByFullName()
    {
        // Arrange
        var targetModelId = 1; // Toyota Camry
        var expectedCount = 6;
        var expectedCustomers = new List<string>
        {
            "Ivanov Ivan Ivanovich",
            "Kuznetsov Alexey Vladimirovich",
            "Petrov Petr Petrovich",
            "Sidorova Anna Sergeevna",
            "Smirnova Ekaterina Dmitrievna",
            "Vasiliev Dmitry Andreevich"
        };

        // Act
        var result = service.Rents
            .Where(r => r.Car.CarModelGeneration.CarModel.Id == targetModelId)
            .Select(r => r.Customer)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        // Assert
        Assert.Equal(expectedCount, result.Count);
        for (var i = 0; i < expectedCount; i++)
        {
            Assert.Equal(expectedCustomers[i], result[i].FullName);
        }
    }

    /// <summary>
    /// Checks for the search of currently rented cars
    /// Checks if only cars with active rental terms are returned
    /// </summary>
    [Fact]
    public void GetCarsCurrentlyRented_ShouldReturnActiveRentals()
    {
        // Arrange
        var currentTime = new DateTime(2024, 3, 3, 14, 0, 0);
        var expectedCount = 1;
        var expectedCarLicensePlate = "F678GH777";

        // Act
        var activeRentals = service.Rents
            .Where(r =>
            {
                var rentalEnd = r.StartTime.AddHours(r.Duration);
                return r.StartTime <= currentTime && currentTime <= rentalEnd;
            })
            .Select(r => r.Car)
            .Distinct()
            .ToList();

        // Assert
        Assert.Equal(expectedCount, activeRentals.Count);
        Assert.Equal(expectedCarLicensePlate, activeRentals[0].LicensePlate);
    }

    /// <summary>
    /// Checks the search for the top 5 most frequently rented cars
    /// Checks the correctness of the ordering by the number of rented cars in descending order
    /// </summary>
    [Fact]
    public void GetTop5MostFrequentlyRentedCars_ShouldReturnExpectedCars()
    {
        // Arrange
        var expectedTopCount = 5;
        var expectedFirstCarLicensePlate = "A123BC777";
        var expectedFirstCarRentalCount = 3;
        var expectedSecondCarLicensePlate = "B234CD777";
        var expectedSecondCarRentalCount = 3;

        // Act
        var topCars = service.Rents
            .GroupBy(r => r.Car)
            .Select(g => new
            {
                Car = g.Key,
                RentalCount = g.Count()
            })
            .OrderByDescending(x => x.RentalCount)
            .ThenBy(x => x.Car.LicensePlate)
            .Take(5)
            .ToList();

        // Assert
        Assert.Equal(expectedTopCount, topCars.Count);
        Assert.Equal(expectedFirstCarLicensePlate, topCars[0].Car.LicensePlate);
        Assert.Equal(expectedFirstCarRentalCount, topCars[0].RentalCount);
        Assert.Equal(expectedSecondCarLicensePlate, topCars[1].Car.LicensePlate);
        Assert.Equal(expectedSecondCarRentalCount, topCars[1].RentalCount);
    }

    /// <summary>
    /// Checks the calculation of the number of rented cars for each car in the fleet
    /// Checks the compliance of aggregate indicators with the records of individual car rentals
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar_ShouldReturnCountForEachCar()
    {
        // Arrange
        var rentals = service.Rents;
        var cars = service.Cars;

        var expectedRentalCounts = new Dictionary<int, int>
        {
            { 1, 3 },   // Car Id = 1 (A123BC777) - 3 rents
            { 2, 3 },   // Car Id = 2 (B234CD777) - 3 rents
            { 3, 2 },   // Car Id = 3 (C345DE777) - 2 rents
            { 4, 1 },   // Car Id = 4 (D456EF777) - 1 rent
            { 5, 1 },   // Car Id = 5 (E567FG777) - 1 rent
            { 6, 1 },   // Car Id = 6 (F678GH777) - 1 rent
            { 7, 1 },   // Car Id = 7 (G789HI777) - 1 rent
            { 8, 1 },   // Car Id = 8 (H890IJ777) - 1 rent
            { 9, 1 },   // Car Id = 9 (I901JK777) - 1 rent
            { 10, 1 },  // Car Id = 10 (J012KL777) - 1 rent
            { 11, 1 },  // Car Id = 11 (K123LM777) - 1 rent
            { 12, 1 },  // Car Id = 12 (L234MN777) - 1 rent
            { 13, 1 },  // Car Id = 13 (M345NO777) - 1 rent
            { 14, 1 },  // Car Id = 14 (N456OP777) - 1 rent
            { 15, 1 }   // Car Id = 15 (O567PQ777) - 1 rent
        };

        // Act
        var rentalCounts = rentals
            .GroupBy(r => r.Car.Id)
            .Select(g => new
            {
                CarId = g.Key,
                RentalCount = g.Count(),
                g.First().Car
            })
            .ToList();

        var totalRentals = rentalCounts.Sum(x => x.RentalCount);

        // Assert
        Assert.Equal(rentals.Count, totalRentals);
        Assert.Equal(expectedRentalCounts, rentalCounts.ToDictionary(x => x.CarId, x => x.RentalCount));
    }

    /// <summary>
    /// Checks the search for the top 5 customers by total rental cost
    /// Checks the correctness of the order by total cost in descending order
    /// </summary>
    [Fact]
    public void GetTop5CustomersByRentalSum_ShouldReturnCorrectOrder()
    {
        // Arrange
        var expectedTopCount = 5;
        var expectedTopCustomerName = "Petrov Petr Petrovich"; // 360.000
        var expectedSecondCustomerName = "Kuznetsov Alexey Vladimirovich"; // 238.800
        var expectedThirdCustomerName = "Sidorova Anna Sergeevna"; // 216.000
        var expectedFourthCustomerName = "Vasiliev Dmitry Andreevich"; // 216.000
        var expectedFifthCustomerName = "Lebedeva Tatyana Petrovna"; // 158.400

        // Act
        var topCustomers = service.Rents
            .GroupBy(r => r.Customer)
            .Select(g => new
            {
                Customer = g.Key,
                TotalRentalCost = g.Sum(r => (decimal)r.Duration * r.Car.CarModelGeneration.RentalCostPerHour)
            })
            .OrderByDescending(x => x.TotalRentalCost)
            .ThenBy(x => x.Customer.FullName)
            .Take(5)
            .ToList();

        // Assert
        Assert.Equal(expectedTopCount, topCustomers.Count);
        Assert.Equal(expectedTopCustomerName, topCustomers[0].Customer.FullName);
        Assert.Equal(expectedSecondCustomerName, topCustomers[1].Customer.FullName);
        Assert.Equal(expectedThirdCustomerName, topCustomers[2].Customer.FullName);
        Assert.Equal(expectedFourthCustomerName, topCustomers[3].Customer.FullName);
        Assert.Equal(expectedFifthCustomerName, topCustomers[4].Customer.FullName);
        Assert.True(topCustomers.All(c => c.TotalRentalCost > 0));
    }

    /// <summary>
    /// Additional test: Get customers by car model name instead of ID
    /// Alternative implementation using model name
    /// </summary>
    [Fact]
    public void GetCustomersByCarModelName_ShouldReturnCorrectCustomers()
    {
        // Arrange
        var targetModelName = "BMW X5";
        var expectedCount = 3;
        var expectedCustomer1 = "Morozov Sergey Viktorovich";
        var expectedCustomer2 = "Nikolaeva Olga Igorevna";
        var expectedCustomer3 = "Vasiliev Dmitry Andreevich";

        // Act
        var customers = service.Rents
            .Where(r => r.Car.CarModelGeneration.CarModel.Name == targetModelName)
            .Select(r => r.Customer)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        // Assert
        Assert.Equal(expectedCount, customers.Count);
        Assert.Equal(expectedCustomer1, customers[0].FullName);
        Assert.Equal(expectedCustomer2, customers[1].FullName);
        Assert.Equal(expectedCustomer3, customers[2].FullName);
    }
}
