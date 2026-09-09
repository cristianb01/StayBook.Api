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

        CreateMap<Booking, ConversationDto>()
            .ForCtorParam(nameof(ConversationDto.Id), opt => opt.MapFrom(src => src.Conversation!.Id))
            .ForCtorParam(nameof(ConversationDto.BookingId), opt => opt.MapFrom(src => src.Conversation!.BookingId))
            .ForCtorParam(nameof(ConversationDto.Messages), opt => opt.MapFrom(src => src.Conversation!.Messages))
            .ForCtorParam(nameof(ConversationDto.Guest), opt => opt.MapFrom(src => src.Guest))
            .ForCtorParam(nameof(ConversationDto.Host), opt => opt.MapFrom(src => src.Host))
            .ForCtorParam(nameof(ConversationDto.CreatedAt), opt => opt.MapFrom(src => src.Conversation!.CreatedAt));
    }
}