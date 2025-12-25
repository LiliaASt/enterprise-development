namespace CarRentalService.Application.Contracts.Clients;

/// <summary>
/// DTO for client responses
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Client identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Driver license number
    /// </summary>
    public string DriverLicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Age calculated from date of birth
    /// </summary>
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}
