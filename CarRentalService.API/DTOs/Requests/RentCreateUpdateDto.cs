using System.ComponentModel.DataAnnotations;

namespace CarRentalService.API.DTOs.Requests;

/// <summary>
/// DTO for creating or updating a rent
/// </summary>
public class RentCreateUpdateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int CarId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ClientId { get; set; }

    [Required]
    public required DateTime StartTime { get; set; }

    [Required]
    [Range(0.5, 720)] // от 30 минут до 30 дней
    public required double Duration { get; set; }
}
