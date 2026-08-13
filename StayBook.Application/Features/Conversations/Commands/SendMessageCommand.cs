using MediatR;

namespace StayBook.Application.Features.Messages.Commands;

public record SendMessageCommand(string Content, int SenderId, int BookingId) : IRequest<Unit>;