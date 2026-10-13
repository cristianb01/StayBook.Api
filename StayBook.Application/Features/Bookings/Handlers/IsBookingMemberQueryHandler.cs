using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Bookings.Handlers;

public class IsBookingMemberQueryHandler : IRequestHandler<IsBookingMemberQuery, bool>
{
    private readonly IBookingRepository _bookingRepository;

    public IsBookingMemberQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<bool> Handle(IsBookingMemberQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
            throw new ResourceNotFoundException("Booking", request.BookingId);

        return booking.IsMember(request.UserId);
    }
}
