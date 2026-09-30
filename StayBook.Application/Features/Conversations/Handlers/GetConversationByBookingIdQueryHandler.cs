using AutoMapper;
using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Conversations.Dtos;
using StayBook.Application.Features.Conversations.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Conversations.Handlers;

public class GetConversationByBookingIdQueryHandler : IRequestHandler<GetConversationBybookingIdQuery, BookingConversationDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetConversationByBookingIdQueryHandler(
        IBookingRepository bookingRepository,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<BookingConversationDto> Handle(GetConversationBybookingIdQuery request, CancellationToken cancellationToken)
    {
        
        var booking = await _bookingRepository.GetForConversationAsync(request.BookingId, cancellationToken);
        
        if (booking is null)
            throw new ResourceNotFoundException("Booking", request.BookingId);
        
        if (!booking.IsMember(request.UserId))
            throw new UnauthorizedConversationAccessException($"User {request.UserId} cannot access conversation for booking {request.BookingId}.");
        
        return _mapper.Map<BookingConversationDto>(booking);
    }
}