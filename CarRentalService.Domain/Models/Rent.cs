namespace CarRentalService.Domain.Models;
/// <summary>
/// Car rental transaction
/// </summary>
public class Rent
{
    /// <summary>
    /// Rental identifier
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Rented car
    /// </summary>
    public required Car Car { get; set; }
    /// <summary>
    /// Customer
    /// </summary>
    public required Customer Customer { get; set; }
    /// <summary>
    /// Rental start time
    /// </summary>
    public required DateTime StartTime { get; set; }
    /// <summary>
    /// Rental duration in hours
    /// </summary>
    public required double Duration { get; set; }
}
