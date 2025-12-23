namespace CarRentalService.API.DTOs.Responses;

/// <summary>
/// DTO for car responses
/// </summary>
public class CarDto
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public CarModelGenerationDto CarModelGeneration { get; set; } = null!;
}

public class CarModelGenerationDto
{
    public int Id { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public int ProductionYear { get; set; }
    public decimal RentalCostPerHour { get; set; }
}
