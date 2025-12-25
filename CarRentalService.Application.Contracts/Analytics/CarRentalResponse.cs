namespace CarRentalService.Application.Contracts.Analytics;

/// <summary>
/// Responses for analytics endpoints
/// </summary>
public class CarRentalResponse
{
    /// <summary>
    /// Car identifier
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// License plate number
    /// </summary>
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Car color
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Car model name
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// Client name
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Rental start time
    /// </summary>
    public DateTime RentalStart { get; set; }

    /// <summary>
    /// Rental end time
    /// </summary>
    public DateTime RentalEnd { get; set; }
}
