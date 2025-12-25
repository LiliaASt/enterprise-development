namespace CarRentalService.Domain.Models;
/// <summary>
/// Car model generation reference
/// </summary>
public class CarModelGeneration
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
    /// Car model
    /// </summary>
    public required CarModel CarModel { get; set; }
    /// <summary>
    /// Production year
    /// </summary>
    public required int ProductionYear { get; set; }
    /// <summary>
    /// Engine volume
    /// </summary>
    public required double EngineVolume { get; set; }
    /// <summary>
    /// Transmission type
    /// </summary>
    public required string TransmissionType { get; set; }
    /// <summary>
    /// Rental cost per hour
    /// </summary>
    public required decimal RentalCostPerHour { get; set; }
}
