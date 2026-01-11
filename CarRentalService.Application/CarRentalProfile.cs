using AutoMapper;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.CarModel;
using CarRentalService.Application.Contracts.CarModelGeneration;
using CarRentalService.Domain.Models;

namespace CarRentalService.Application;

/// <summary>
/// AutoMapper profile for Car Rental Service domain entities to DTOs mapping
/// </summary>
public class CarRentalProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CarRentalProfile class
    /// </summary>
    public CarRentalProfile()
    {
        // CarModel -> CarModelDto
        CreateMap<CarModel, CarModelDto>();

        // CarModelCreateUpdateDto -> CarModel
        CreateMap<CarModelCreateUpdateDto, CarModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // CarModelGeneration -> CarModelGenerationDto
        CreateMap<CarModelGeneration, CarModelGenerationDto>()
            .ForMember(dest => dest.CarModelName, opt => opt.MapFrom(src => src.CarModel.Name));

        // CarModelGenerationCreateUpdateDto -> CarModelGeneration
        CreateMap<CarModelGenerationCreateUpdateDto, CarModelGeneration>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Car -> CarDto
        CreateMap<Car, CarDto>()
            .ForMember(dest => dest.CarModelGeneration, opt => opt.MapFrom(src => src.CarModelGeneration));

        // CarCreateUpdateDto -> Car
        CreateMap<CarCreateUpdateDto, Car>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Customer -> ClientDto
        CreateMap<Customer, ClientDto>();

        // ClientCreateUpdateDto -> Customer
        CreateMap<ClientCreateUpdateDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Rent -> RentDto
        CreateMap<Rent, RentDto>()
            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => new Contracts.Shared.CarSimpleDto
            {
                Id = src.Car.Id,
                LicensePlate = src.Car.LicensePlate,
                ModelName = src.Car.CarModelGeneration.CarModel.Name,
                RentalCostPerHour = src.Car.CarModelGeneration.RentalCostPerHour
            }))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => new Contracts.Shared.ClientSimpleDto
            {
                Id = src.Customer.Id,
                FullName = src.Customer.FullName
            }));

        // RentCreateUpdateDto -> Rent
        CreateMap<RentCreateUpdateDto, Rent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
