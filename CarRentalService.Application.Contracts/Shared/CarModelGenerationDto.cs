namespace CarRentalService.Application.Contracts.Shared;

/// <summary>
/// DTO for car model generation responses
/// </summary>
public class CarModelGenerationDto
{
    /// <summary>
    /// Generation identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Model name
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// Production year
    /// </summary>
    public int ProductionYear { get; set; }

    /// <summary>
    /// Rental cost per hour
    /// </summary>
    public decimal RentalCostPerHour { get; set; }
}
