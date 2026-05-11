using AutoMapper;
using StayBook.Application.Features.Properties.Dtos;
using StayBook.Domain.Bookings;
using StayBook.Domain.Properties;

namespace StayBook.Application.Mappings;

public class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        CreateMap<Property, PropertyDto>()
            .ConstructUsing(src => new PropertyDto(
                src.Id,
                src.Name,
                src.Description,
                src.HostId));
    }
}