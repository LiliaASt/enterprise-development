namespace CarRentalService.Domain.Models;
/// <summary>
/// Car model reference
/// </summary>
public class CarModel
{
    /// <summary>
    /// Model identifier
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Model name
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Drive type
    /// </summary>
    public required string DriveType { get; set; }
    /// <summary>
    /// Number of seats
    /// </summary>
    public required int SeatCount { get; set; }
    /// <summary>
    /// Body type
    /// </summary>
    public required string BodyType { get; set; }
    /// <summary>
    /// Car class
    /// </summary>
    public required string CarClass { get; set; }
}
