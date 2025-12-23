using System.ComponentModel.DataAnnotations;

namespace CarRentalService.API.DTOs.Requests;

/// <summary>
/// DTO for creating or updating a client
/// </summary>
public class ClientCreateUpdateDto
{
    [Required]
    [StringLength(50)]
    public required string DriverLicenseNumber { get; set; }

    [Required]
    [StringLength(100)]
    public required string FullName { get; set; }

    [Required]
    public required DateTime DateOfBirth { get; set; }
}
