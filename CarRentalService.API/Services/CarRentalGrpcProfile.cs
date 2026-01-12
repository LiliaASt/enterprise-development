using AutoMapper;
using CarRentalService.Application.Contracts.Grpc;
using CarRentalService.Application.Contracts.Rents;

namespace CarRentalService.API.Services;

/// <summary>
/// AutoMapper profile for mapping gRPC messages to DTOs
/// </summary>
public class CarRentalGrpcProfile : Profile
{
    public CarRentalGrpcProfile()
    {
        CreateMap<RentalContractMessage, RentCreateUpdateDto>()
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => DateTime.Parse(src.StartTime)))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.DurationHours));
    }
}
