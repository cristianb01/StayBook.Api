using MediatR;

namespace StayBook.Application.Features.Bookings.Commands;

public record ConfirmBookingCommand(
    int BookingId,
    string PaymentReferenceId) : IRequest<Unit>;