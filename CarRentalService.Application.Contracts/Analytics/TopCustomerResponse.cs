namespace CarRentalService.Application.Contracts.Analytics;

/// <summary>
/// Response for top customers by revenue analytics
/// </summary>
public class TopCustomerResponse
{
    /// <summary>
    /// Customer identifier
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Number of rentals
    /// </summary>
    public int RentalCount { get; set; }

    /// <summary>
    /// Total revenue
    /// </summary>
    public decimal TotalRevenue { get; set; }
}
