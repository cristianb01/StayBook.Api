using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Messages.Commands;
using StayBook.Application.Interfaces;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Features.Conversations.Handlers;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
{
    private readonly IBookingRepository _bookingRepository;

    public SendMessageCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdWithPropertyAsync(request.BookingId, cancellationToken);

        if (booking is null)
            throw new BookingNotFoundException(request.BookingId);
        
        if (!booking.CanSendMessage(request.SenderId))
            throw new UnauthorizedConversationAccessException($"{request.SenderId} cannot send messages in this booking.");

        var conversation = booking.Conversation ?? booking.StartConversation(); 

        conversation.AddMessage(request.SenderId, request.Content);
        
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return Unit.Value;
    }
}