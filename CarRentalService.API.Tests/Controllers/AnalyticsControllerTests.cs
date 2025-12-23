using CarRentalService.API.Controllers;
using CarRentalService.API.Interfaces;
using CarRentalService.API.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRentalService.API.Tests.Controllers;

public class AnalyticsControllerTests
{
    private readonly Mock<IAnalyticsService> _mockAnalyticsService;
    private readonly AnalyticsController _controller;

    public AnalyticsControllerTests()
    {
        _mockAnalyticsService = new Mock<IAnalyticsService>();
        _controller = new AnalyticsController(_mockAnalyticsService.Object);
    }

    [Fact]
    public void GetClientsByModelName_ReturnsOkResult()
    {
        // Arrange
        var modelName = "Toyota Camry";
        var expected = new List<string> { "Ivanov Ivan", "Petrov Petr" };
        _mockAnalyticsService.Setup(s => s.ReadCustomersByModelName(modelName))
            .Returns(expected);

        // Act
        var result = _controller.GetClientsByModelName(modelName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(expected, returned);
    }

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

    [Fact]
    public void GetTopCustomersByRevenue_ReturnsOkResult()
    {
        // Arrange
        var expected = new List<TopCustomerResponse>
        {
            new() { CustomerId = 1, FullName = "Petrov Petr", TotalRevenue = 360000 }
        };
        _mockAnalyticsService.Setup(s => s.ReadTopCustomersByTotalAmount(5))
            .Returns(expected);

        // Act
        var result = _controller.GetTopCustomersByRevenue();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<List<TopCustomerResponse>>(okResult.Value);
        Assert.Single(returned);
        Assert.Equal(360000, returned[0].TotalRevenue);
    }
}
