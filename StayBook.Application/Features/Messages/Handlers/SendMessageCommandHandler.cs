using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Messages.Commands;
using StayBook.Application.Interfaces;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Features.Messages.Handlers;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Message>
{
    private readonly IBookingRepository _bookingRepository;

    public SendMessageCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Message> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdWithConversationAsync(request.BookingId, cancellationToken);

        if (booking is null)
            throw new ResourceNotFoundException("Booking", request.BookingId);
        
        if (!booking.IsMember(request.SenderId))
            throw new UnauthorizedConversationAccessException($"{request.SenderId} cannot send messages in this booking.");

        var conversation = booking.Conversation ?? booking.StartConversation(); 

        var createdMessage = conversation.AddMessage(request.SenderId, request.Content);
        
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return createdMessage;
    }
}