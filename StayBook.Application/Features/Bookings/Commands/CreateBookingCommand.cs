using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Bookings.Commands;

public record CreateBookingCommand(
    int UserId,
    int PropertyId,
    DateTime StartDate,
    DateTime EndDate) : ICommand<int>;