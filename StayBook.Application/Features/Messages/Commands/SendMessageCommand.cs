using MediatR;
using StayBook.Domain.Conversations;

namespace StayBook.Application.Features.Messages.Commands;

public record SendMessageCommand(string Content, int SenderId, int BookingId) : IRequest<Message>;