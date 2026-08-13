using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StayBook.Api.Models;
using StayBook.Application.Features.Messages.Commands;

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
        if (!TryGetCurrentUserId(out var senderId))
        {
            return Unauthorized();
        }

        var command = new SendMessageCommand(request.Content, senderId, bookingId);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        var claimValue = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claimValue, out userId);
    }
}