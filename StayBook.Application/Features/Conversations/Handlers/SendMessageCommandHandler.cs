using MediatR;
using StayBook.Application.Features.Messages.Commands;

namespace StayBook.Application.Features.Conversations.Handlers;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
{
    public Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}