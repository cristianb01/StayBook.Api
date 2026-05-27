using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Interfaces;
using StayBook.Domain.Bookings;
using StayBook.Domain.ValueObjects;

namespace StayBook.Application.Features.Bookings.Handlers;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
{
    private readonly IBookingRepository _bookingRepository;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // TODO: Lock the property row in the database while checking availability and creating the booking.
        var hasOverlap = await _bookingRepository.HasOverlapAsync(request.PropertyId, request.StartDate, request.EndDate, cancellationToken);

        if (hasOverlap)
        { 
            throw new BookingOverlayException();
        }
        
        var booking = new Booking(request.UserId, request.PropertyId, new DateRange(request.StartDate, request.EndDate), 0);
        
        await _bookingRepository.AddAsync(booking, cancellationToken);
        return booking.Id;
    }
}