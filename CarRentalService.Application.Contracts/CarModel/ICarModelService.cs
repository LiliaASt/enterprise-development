using CarRentalService.Application.Contracts.Cars;

namespace CarRentalService.Application.Contracts.CarModel;

/// <summary>
/// Car model service interface
/// </summary>
public interface ICarModelService : IApplicationService<CarModelDto, CarModelCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves all cars associated with a specific car model
    /// </summary>
    public Task<IList<CarDto>> GetCarsByModelAsync(int modelId);
}
