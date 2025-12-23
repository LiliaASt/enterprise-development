namespace CarRentalService.API.DTOs.Responses;

/// <summary>
/// Responses for analytics endpoints
/// </summary>
public class CarRentalResponse
{
    public int CarId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public DateTime RentalStart { get; set; }
    public DateTime RentalEnd { get; set; }
}

public class TopCarResponse
{
    public int CarId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public int RentalCount { get; set; }
    public double TotalHours { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class CarRentalCountResponse
{
    public int CarId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public int RentalCount { get; set; }
}

public class TopCustomerResponse
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int RentalCount { get; set; }
    public decimal TotalRevenue { get; set; }
}
