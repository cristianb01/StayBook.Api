using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayBook.Api.Models;
using StayBook.Application.Features.Messages.Commands;
using StayBook.Domain.Conversations;
using StayBook.Extensions;

namespace StayBook.Controllers;

[ApiController]
[Route("api/v1")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("bookings/{bookingId:int}/conversation/messages")]
    public async Task<ActionResult<Message>> SendMessage([FromRoute] int bookingId,
        [FromBody] SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var senderId = User.TryGetCurrentUserId();

        var command = new SendMessageCommand(request.Content, senderId, bookingId);
        var message = await _mediator.Send(command, cancellationToken);
        return Ok(message);
    }
    
}