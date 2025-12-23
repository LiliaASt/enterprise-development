namespace CarRentalService.API.DTOs.Responses;

/// <summary>
/// DTO for client responses
/// </summary>
public class ClientDto
{
    public int Id { get; set; }
    public string DriverLicenseNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}
