namespace CarRentalService.Api.Configuration;

/// <summary>
/// Configuration options for the rental generator service
/// </summary>
public class RentalGeneratorOptions
{
    public const string SectionName = "RentalGenerator";

    /// <summary>
    /// Default number of rental contracts to generate
    /// </summary>
    public int Count { get; set; } = 100;

    /// <summary>
    /// Batch size for streaming rental contracts
    /// </summary>
    public int BatchSize { get; set; } = 10;

    /// <summary>
    /// Retry delay in seconds for reconnection attempts
    /// </summary>
    public int RetryDelay { get; set; } = 5;

    /// <summary>
    /// gRPC service address for the rental generator
    /// </summary>
    public string? GrpcAddress { get; set; }
}
