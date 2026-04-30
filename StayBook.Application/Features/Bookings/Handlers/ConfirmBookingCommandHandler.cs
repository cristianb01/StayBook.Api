using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Interfaces;
using StayBook.Domain.Enums;

namespace StayBook.Application.Features.Bookings.Handlers;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Unit>
{
    private readonly IBookingRepository  _bookingRepository;
    
    public ConfirmBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public async Task<Unit> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var existingBooking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (existingBooking == null)
        {
            throw new BookingNotFoundException(request.BookingId);
        }

        if (existingBooking.Status != BookingStatus.Pending)
        {
            throw new InvalidBookingStatusException(request.BookingId, existingBooking.Status, BookingStatus.Pending);
        }

        existingBooking.Confirm();
        await _bookingRepository.UpdateAsync(existingBooking, cancellationToken);

        return Unit.Value;
    }
}