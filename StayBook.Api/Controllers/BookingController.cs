using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayBook.Api.Models;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Features.Bookings.Queries;
using StayBook.Application.Models;
using StayBook.Extensions;

namespace StayBook.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        if (!User.TryGetCurrentUserId(out var senderId))
        {
            return Unauthorized();
        }
        
        var command = new CreateBookingCommand(
            senderId,
            request.PropertyId,
            request.StartDate,
            request.EndDate);

        var bookingId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateBooking), new { id = bookingId }, bookingId);
    }

    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> ConfirmBooking(int id, ConfirmBookingRequest confirmBookingRequest, CancellationToken cancellationToken)
    {
        var command = new ConfirmBookingCommand(id, confirmBookingRequest.PaymentReferenceId);
        
        await _mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookings(
        [FromQuery] PaginationFilters paginationFilters, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllBookingsQuery(paginationFilters);
        var bookings = await _mediator.Send(query, cancellationToken);
        return Ok(bookings);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBooking([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetBookingByIdQuery(id);
        var booking = await _mediator.Send(query, cancellationToken);
        return Ok(booking);
    }
}