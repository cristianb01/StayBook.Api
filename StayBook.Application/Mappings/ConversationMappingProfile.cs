using AutoMapper;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Domain.Bookings;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Mappings;

public class ConversationMappingProfile : Profile
{
    public ConversationMappingProfile()
    {
        CreateMap<Conversation, ConversationDto>();

        CreateMap<Booking, BookingConversationDto>()
            .ForCtorParam(nameof(BookingConversationDto.BookingId), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(BookingConversationDto.Guest), opt => opt.MapFrom(src => src.Guest))
            .ForCtorParam(nameof(BookingConversationDto.Host), opt => opt.MapFrom(src => src.Host))
            .ForCtorParam(nameof(BookingConversationDto.Conversation), opt => opt.MapFrom(src => src.Conversation));
    }
}