namespace CarRentalService.Domain.Models;
/// <summary>
/// Car in the rental fleet
/// </summary>
public class Car
{
    /// <summary>
    /// Car identifier
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// License plate number
    /// </summary>
    public required string LicensePlate { get; set; }
    /// <summary>
    /// Car color
    /// </summary>
    public required string Color { get; set; }
    /// <summary>
    /// Car model generation identifier
    /// </summary>
    public int CarModelGenerationId { get; set; }
    /// <summary>
    /// Car model generation
    /// </summary>
    public required CarModelGeneration CarModelGeneration { get; set; }
}
