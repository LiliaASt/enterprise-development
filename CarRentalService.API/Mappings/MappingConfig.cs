using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.Shared;
using CarRentalService.Domain.Models;
using Mapster;

namespace CarRentalService.API.Mappings;

/// <summary>
/// Mapster configuration
/// </summary>
public class MappingConfig : IRegister
{
    /// <summary>
    /// Configures mapping rules between domain models and DTOs
    /// </summary>
    /// <param name="config">TypeAdapterConfig instance to configure mappings</param>
    public void Register(TypeAdapterConfig config)
    {
        // Car -> CarDto mapping configuration
        config.NewConfig<Car, CarDto>()
            .Map(dest => dest.CarModelGeneration, src => new CarModelGenerationDto
            {
                Id = src.CarModelGeneration.Id,
                ModelName = src.CarModelGeneration.CarModel.Name,
                ProductionYear = src.CarModelGeneration.ProductionYear,
                RentalCostPerHour = src.CarModelGeneration.RentalCostPerHour
            });

        // Customer -> ClientDto mapping configuration
        config.NewConfig<Customer, ClientDto>();

        // Rent -> RentDto mapping configuration
        config.NewConfig<Rent, RentDto>()
            .Map(dest => dest.Car, src => new CarSimpleDto
            {
                Id = src.Car.Id,
                LicensePlate = src.Car.LicensePlate,
                ModelName = src.Car.CarModelGeneration.CarModel.Name,
                RentalCostPerHour = src.Car.CarModelGeneration.RentalCostPerHour
            })
            .Map(dest => dest.Client, src => new ClientSimpleDto
            {
                Id = src.Customer.Id,
                FullName = src.Customer.FullName
            });
    }
}
