namespace CarRentalService.API.DTOs.Responses;

/// <summary>
/// DTO for rent responses
/// </summary>
public class RentDto
{
    public int Id { get; set; }
    public CarSimpleDto Car { get; set; } = null!;
    public ClientSimpleDto Client { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public double Duration { get; set; }
    public DateTime EndTime => StartTime.AddHours(Duration);
    public decimal TotalCost => (decimal)Duration * Car.RentalCostPerHour;
}

public class CarSimpleDto
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public decimal RentalCostPerHour { get; set; }
}

public class ClientSimpleDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}
