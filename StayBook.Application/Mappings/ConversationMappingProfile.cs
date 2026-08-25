using AutoMapper;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Mappings;

public class ConversationMappingProfile : Profile
{
    public ConversationMappingProfile()
    {
        CreateMap<Conversation, ConversationDto>();
    }
}