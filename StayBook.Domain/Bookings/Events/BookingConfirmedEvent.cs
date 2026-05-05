using StayBook.Domain.Events;

namespace StayBook.Domain.Bookings.Events;

public class BookingConfirmedEvent(int bookingId) : DomainEvent
{
    public int BookingId { get; private set; } = bookingId;
}