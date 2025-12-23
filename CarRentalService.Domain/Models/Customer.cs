namespace CarRentalService.Domain.Models;
/// <summary>
/// Customer of the rental service
/// </summary>
public class Customer
{
    /// <summary>
    /// Customer identifier
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Driver license number
    /// </summary>
    public required string DriverLicenseNumber { get; set; }
    /// <summary>
    /// Full name
    /// </summary>
    public required string FullName { get; set; }
    /// <summary>
    /// Date of birth
    /// </summary>
    public required DateTime DateOfBirth { get; set; }
}
