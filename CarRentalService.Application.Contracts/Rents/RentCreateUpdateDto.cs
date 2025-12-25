using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Application.Contracts.Rents;

/// <summary>
/// DTO for creating or updating a rent
/// </summary>
public class RentCreateUpdateDto
{
    /// <summary>
    /// Car identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int CarId { get; set; }

    /// <summary>
    /// Client identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int ClientId { get; set; }

    /// <summary>
    /// Rental start time
    /// </summary>
    [Required]
    public required DateTime StartTime { get; set; }

    /// <summary>
    /// Rental duration in hours (from 30 minutes to 30 days)
    /// </summary>
    [Required]
    [Range(0.5, 720)]
    public required double Duration { get; set; }
}
