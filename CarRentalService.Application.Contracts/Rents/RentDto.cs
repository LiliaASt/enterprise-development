using CarRentalService.Application.Contracts.Shared;

namespace CarRentalService.Application.Contracts.Rents;

/// <summary>
/// DTO for rent responses
/// </summary>
public class RentDto
{
    /// <summary>
    /// Rental identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Car information
    /// </summary>
    public CarSimpleDto Car { get; set; } = null!;

    /// <summary>
    /// Client information
    /// </summary>
    public ClientSimpleDto Client { get; set; } = null!;

    /// <summary>
    /// Rental start time
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Rental duration in hours
    /// </summary>
    public double Duration { get; set; }

    /// <summary>
    /// Rental end time (calculated)
    /// </summary>
    public DateTime EndTime => StartTime.AddHours(Duration);

    /// <summary>
    /// Total cost (calculated)
    /// </summary>
    public decimal TotalCost => (decimal)Duration * Car.RentalCostPerHour;
}
