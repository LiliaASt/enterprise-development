namespace CarRentalService.Application.Contracts.CarModelGeneration;

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
    /// Car model identifier
    /// </summary>
    public int CarModelId { get; set; }

    /// <summary>
    /// Car model name
    /// </summary>
    public string CarModelName { get; set; } = string.Empty;

    /// <summary>
    /// Production year
    /// </summary>
    public int ProductionYear { get; set; }

    /// <summary>
    /// Engine volume in liters
    /// </summary>
    public double EngineVolume { get; set; }

    /// <summary>
    /// Transmission type
    /// </summary>
    public string TransmissionType { get; set; } = string.Empty;

    /// <summary>
    /// Rental cost per hour
    /// </summary>
    public decimal RentalCostPerHour { get; set; }
}
