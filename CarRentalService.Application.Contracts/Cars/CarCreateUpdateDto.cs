using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Application.Contracts.Cars;

/// <summary>
/// DTO for creating or updating a car
/// </summary>
public class CarCreateUpdateDto
{
    /// <summary>
    /// License plate number
    /// </summary>
    [Required]
    [StringLength(20)]
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Car color
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string Color { get; set; }

    /// <summary>
    /// Car model generation identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int CarModelGenerationId { get; set; }
}
