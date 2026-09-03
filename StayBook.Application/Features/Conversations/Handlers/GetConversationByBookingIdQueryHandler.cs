using AutoMapper;
using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Conversations.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Conversations.Handlers;

public class GetConversationByBookingIdQueryHandler : IRequestHandler<GetConversationBybookingIdQuery, ConversationDto>
{
    private readonly IConversationsQueries _conversationsQueries;
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetConversationByBookingIdQueryHandler(
        IConversationsQueries conversationsQueries,
        IBookingRepository bookingRepository,
        IMapper mapper)
    {
        _conversationsQueries = conversationsQueries;
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<ConversationDto> Handle(GetConversationBybookingIdQuery request, CancellationToken cancellationToken)
    {
        
        var booking = await _bookingRepository.GetForConversationAsync(request.BookingId, cancellationToken);
        
        if (booking is null)
            throw new ResourceNotFoundException("Booking", request.BookingId);
        
        if (!booking.IsMember(request.UserId))
            throw new UnauthorizedConversationAccessException($"User {request.UserId} cannot access conversation for booking {request.BookingId}.");
        
        if (booking.Conversation is null)
            throw new ResourceNotFoundException("Conversation", request.BookingId);
        
        return _mapper.Map<ConversationDto>(booking.Conversation);
    }
}