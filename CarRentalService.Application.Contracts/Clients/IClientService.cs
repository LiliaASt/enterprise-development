namespace CarRentalService.Application.Contracts.Clients;

/// <summary>
/// Client service interface
/// </summary>
public interface IClientService : IApplicationService<ClientDto, ClientCreateUpdateDto, int>
{
}
