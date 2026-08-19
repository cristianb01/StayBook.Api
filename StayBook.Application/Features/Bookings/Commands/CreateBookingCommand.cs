using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Bookings.Commands;

public record CreateBookingCommand(
    int GuestId,
    int PropertyId,
    DateTime StartDate,
    DateTime EndDate) : ICommand<int>;