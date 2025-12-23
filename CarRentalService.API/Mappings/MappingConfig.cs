using CarRentalService.API.DTOs.Requests;
using CarRentalService.API.DTOs.Responses;
using CarRentalService.Domain.Models;
using Mapster;

namespace CarRentalService.API.Mappings;

/// <summary>
/// Mapster configuration
/// </summary>
public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Car mappings
        config.NewConfig<Car, CarDto>()
            .Map(dest => dest.CarModelGeneration, src => new CarModelGenerationDto
            {
                Id = src.CarModelGeneration.Id,
                ModelName = src.CarModelGeneration.CarModel.Name,
                ProductionYear = src.CarModelGeneration.ProductionYear,
                RentalCostPerHour = src.CarModelGeneration.RentalCostPerHour
            });

        config.NewConfig<CarCreateUpdateDto, Car>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CarModelGeneration);

        // Customer/Client mappings
        config.NewConfig<Customer, ClientDto>();

        config.NewConfig<ClientCreateUpdateDto, Customer>()
            .Ignore(dest => dest.Id);

        // Rent mappings
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

        config.NewConfig<RentCreateUpdateDto, Rent>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Car)
            .Ignore(dest => dest.Customer);
    }
}
