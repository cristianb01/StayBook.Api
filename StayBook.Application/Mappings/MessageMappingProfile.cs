using AutoMapper;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Mappings;

public class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<Message, MessageDto>();
    }
}