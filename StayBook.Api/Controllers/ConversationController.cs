using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayBook.Api.Models;
using StayBook.Application.Features.Messages.Commands;
using StayBook.Extensions;

namespace StayBook.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ConversationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{bookingId:int}")]
    public async Task<IActionResult> Create(
        [FromRoute] int bookingId, 
        [FromBody] SendMessageRequest request, 
        CancellationToken cancellationToken)
    {
        if (!User.TryGetCurrentUserId(out var senderId))
        {
            return Unauthorized();
        }

        var command = new SendMessageCommand(request.Content, senderId, bookingId);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}