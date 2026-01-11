using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Application.Contracts.CarModel;

/// <summary>
/// DTO for creating or updating a car model
/// </summary>
public class CarModelCreateUpdateDto
{
    /// <summary>
    /// Model name
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Drive type
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string DriveType { get; set; }

    /// <summary>
    /// Number of seats
    /// </summary>
    [Required]
    [Range(1, 20)]
    public required int SeatCount { get; set; }

    /// <summary>
    /// Body type
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string BodyType { get; set; }

    /// <summary>
    /// Car class
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string CarClass { get; set; }
}
