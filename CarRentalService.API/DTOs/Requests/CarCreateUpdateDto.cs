using System.ComponentModel.DataAnnotations;

namespace CarRentalService.API.DTOs.Requests;

/// <summary>
/// DTO for creating or updating a car
/// </summary>
public class CarCreateUpdateDto
{
    [Required]
    [StringLength(20)]
    public required string LicensePlate { get; set; }

    [Required]
    [StringLength(50)]
    public required string Color { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CarModelGenerationId { get; set; }
}
