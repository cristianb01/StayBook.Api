using AutoMapper;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Domain.Users;

namespace StayBook.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}