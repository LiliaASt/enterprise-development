namespace CarRentalService.Application.Contracts.Analytics;

/// <summary>
/// Response for car rental count analytics
/// </summary>
public class CarRentalCountResponse
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
    /// Car model name
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// Number of rentals
    /// </summary>
    public int RentalCount { get; set; }
}
