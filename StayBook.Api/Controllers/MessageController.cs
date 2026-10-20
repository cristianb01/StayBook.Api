using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StayBook.Api.Models;
using StayBook.Application.Features.Messages.Commands;
using StayBook.Domain.Conversations;
using StayBook.Extensions;
using StayBook.Hubs;

namespace StayBook.Controllers;

[ApiController]
[Route("api/v1")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<ChatHub>  _chatHub;

    public MessageController(IMediator mediator, IHubContext<ChatHub> chatHub)
    {
        _mediator = mediator;
        _chatHub = chatHub;
    }

    [HttpPost("bookings/{bookingId:int}/conversation/messages")]
    public async Task<ActionResult<Message>> SendMessage([FromRoute] int bookingId,
        [FromBody] SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var senderId = User.TryGetCurrentUserId();

        var command = new SendMessageCommand(request.Content, senderId, bookingId);
        var message = await _mediator.Send(command, cancellationToken); // This already checks if user is member

        var notification = new MessageReceivedDto(message.Id, bookingId, senderId, message.Content, message.CreatedAt);
        
        await _chatHub.Clients
            .Group(ChatHub.GetGroupName(bookingId))
            .SendAsync("MessageReceived", notification, cancellationToken);
        
        return Ok(message);
    }
    
}