namespace CarRentalService.Application.Contracts.CarModel;

/// <summary>
/// DTO for car model responses
/// </summary>
public class CarModelDto
{
    /// <summary>
    /// Model identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Model name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Drive type
    /// </summary>
    public string DriveType { get; set; } = string.Empty;

    /// <summary>
    /// Number of seats
    /// </summary>
    public int SeatCount { get; set; }

    /// <summary>
    /// Body type
    /// </summary>
    public string BodyType { get; set; } = string.Empty;

    /// <summary>
    /// Car class
    /// </summary>
    public string CarClass { get; set; } = string.Empty;
}
