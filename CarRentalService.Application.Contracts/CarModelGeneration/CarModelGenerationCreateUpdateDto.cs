using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Application.Contracts.CarModelGeneration;

/// <summary>
/// DTO for creating or updating a car model generation
/// </summary>
public class CarModelGenerationCreateUpdateDto
{
    /// <summary>
    /// Car model identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int CarModelId { get; set; }

    /// <summary>
    /// Production year
    /// </summary>
    [Required]
    [Range(1900, 2100)]
    public required int ProductionYear { get; set; }

    /// <summary>
    /// Engine volume in liters
    /// </summary>
    [Required]
    [Range(0.5, 10.0)]
    public required double EngineVolume { get; set; }

    /// <summary>
    /// Transmission type
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string TransmissionType { get; set; }

    /// <summary>
    /// Rental cost per hour
    /// </summary>
    [Required]
    [Range(0.01, 10000)]
    public required decimal RentalCostPerHour { get; set; }
}
