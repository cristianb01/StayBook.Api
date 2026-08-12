using MediatR;

namespace StayBook.Application.Features.Messages.Commands;

public record SendMessageCommand(string Message, int SenderId, int BookingId) : IRequest<Unit>;