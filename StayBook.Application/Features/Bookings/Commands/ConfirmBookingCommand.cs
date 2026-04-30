using MediatR;
using StayBook.Domain.Entities;

namespace StayBook.Application.Features.Bookings.Commands;

public record ConfirmBookingCommand(int BookingId) : IRequest<Unit>;