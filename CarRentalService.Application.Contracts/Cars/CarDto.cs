using CarRentalService.Application.Contracts.CarModelGeneration;

namespace CarRentalService.Application.Contracts.Cars;

/// <summary>
/// DTO for car responses
/// </summary>
public class CarDto
{
    /// <summary>
    /// Car identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// License plate number
    /// </summary>
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Car color
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Car model generation information
    /// </summary>
    public CarModelGenerationDto CarModelGeneration { get; set; } = null!;
}
