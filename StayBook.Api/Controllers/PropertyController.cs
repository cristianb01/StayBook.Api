
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StayBook.Application.Features.Properties.Queries;

namespace StayBook.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableBookings(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        CancellationToken cancellationToken)
    {
        var query = new GetAvailablePropertiesQuery(startDate, endDate);
        var properties = await _mediator.Send(query, cancellationToken);
        return Ok(properties);
    }
}