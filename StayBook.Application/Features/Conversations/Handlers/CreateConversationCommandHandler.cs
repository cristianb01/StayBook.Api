using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Conversations.Commands;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Conversations.Handlers;

public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, int>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IBookingRepository _bookingRepository;

    public CreateConversationCommandHandler(IConversationRepository conversationRepository, IBookingRepository bookingRepository)
    {
        _conversationRepository = conversationRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<int> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        var existingBooking = await _bookingRepository.GetByIdWithConversationAsync(request.BookingId, cancellationToken);

        if (existingBooking is null)
            throw new ResourceNotFoundException("Booking", request.BookingId);

        if (!existingBooking.IsMember(request.UserId))
            throw new UnauthorizedConversationAccessException($"{request.UserId} is not part of this booking.");

        existingBooking.StartConversation();

        await _bookingRepository.UpdateAsync(existingBooking, cancellationToken);
        
        return existingBooking.Conversation!.Id;
    }
}