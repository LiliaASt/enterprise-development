namespace CarRentalService.Application.Contracts.Shared;

/// <summary>
/// Simplified DTO for car information
/// </summary>
public class CarSimpleDto
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
    /// Model name
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// Rental cost per hour
    /// </summary>
    public decimal RentalCostPerHour { get; set; }
}
