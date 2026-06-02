using MediatR;
using StayBook.Application.Features.Bookings.DTOs;

namespace StayBook.Application.Features.Bookings.Queries;

public record GetBookingByIdQuery(int Id) : IRequest<BookingDto>;