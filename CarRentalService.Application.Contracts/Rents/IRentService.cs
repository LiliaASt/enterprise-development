namespace CarRentalService.Application.Contracts.Rents;

/// <summary>
/// Rent service interface
/// </summary>
public interface IRentService : IApplicationService<RentDto, RentCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves all rental transactions for a specific client
    /// </summary>
    public Task<IList<RentDto>> GetRentalsByClientAsync(int clientId);

    /// <summary>
    /// Retrieves all rental transactions for a specific car
    /// </summary>
    public Task<IList<RentDto>> GetRentalsByCarAsync(int carId);
}
