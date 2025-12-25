using CarRentalService.API.Controllers;
using CarRentalService.Application.Contracts;
using CarRentalService.Application.Contracts.Analytics;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRentalService.API.Tests.Controllers;

/// <summary>
/// Unit tests for AnalyticsController
/// </summary>
public class AnalyticsControllerTests
{
    private readonly Mock<IAnalyticsService> _mockAnalyticsService;
    private readonly AnalyticsController _controller;

    /// <summary>
    /// Initializes test dependencies
    /// </summary>
    public AnalyticsControllerTests()
    {
        _mockAnalyticsService = new Mock<IAnalyticsService>();
        _controller = new AnalyticsController(_mockAnalyticsService.Object);
    }

    /// <summary>
    /// Tests that GetClientsByModelName returns OkObjectResult with correct data
    /// </summary>
    [Fact]
    public void GetClientsByModelName_ReturnsOkResult()
    {
        // Arrange
        var modelName = "Toyota Camry";
        var expected = new List<string> { "Ivanov Ivan Ivanovich", "Petrov Petr Petrovich" };
        _mockAnalyticsService.Setup(s => s.ReadCustomersByModelName(modelName))
            .Returns(expected);

        // Act
        var result = _controller.GetClientsByModelName(modelName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(expected, returned);
    }

    /// <summary>
    /// Tests that GetTopRentedCars returns OkObjectResult with correct data
    /// </summary>
    [Fact]
    public void GetTopRentedCars_ReturnsOkResult()
    {
        // Arrange
        var expected = new List<TopCarResponse>
        {
            new() { CarId = 1, LicensePlate = "A123BC777", RentalCount = 3 }
        };
        _mockAnalyticsService.Setup(s => s.ReadTopMostRentedCars(5))
            .Returns(expected);

        // Act
        var result = _controller.GetTopRentedCars();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<List<TopCarResponse>>(okResult.Value);
        Assert.Single(returned);
    }
}
