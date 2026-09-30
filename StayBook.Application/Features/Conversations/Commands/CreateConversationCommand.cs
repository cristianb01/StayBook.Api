using MediatR;

namespace StayBook.Application.Features.Conversations.Commands;

public record CreateConversationCommand(int BookingId, int UserId) : IRequest<int>;