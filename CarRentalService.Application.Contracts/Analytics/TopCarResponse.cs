namespace CarRentalService.Application.Contracts.Analytics;

/// <summary>
/// Response for top rented cars analytics
/// </summary>
public class TopCarResponse
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

    /// <summary>
    /// Total rental hours
    /// </summary>
    public double TotalHours { get; set; }

    /// <summary>
    /// Total revenue
    /// </summary>
    public decimal TotalRevenue { get; set; }
}
