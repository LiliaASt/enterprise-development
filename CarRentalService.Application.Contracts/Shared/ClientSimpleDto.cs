namespace CarRentalService.Application.Contracts.Shared;

/// <summary>
/// Simplified DTO for client information
/// </summary>
public class ClientSimpleDto
{
    /// <summary>
    /// Client identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
