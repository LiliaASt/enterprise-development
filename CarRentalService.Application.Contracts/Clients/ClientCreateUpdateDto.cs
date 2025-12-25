using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Application.Contracts.Clients;

/// <summary>
/// DTO for creating or updating a client
/// </summary>
public class ClientCreateUpdateDto
{
    /// <summary>
    /// Driver license number
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string DriverLicenseNumber { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string FullName { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    [Required]
    public required DateTime DateOfBirth { get; set; }
}
