using MediatR;

namespace StayBook.Application.Features.Bookings.Queries;

public record IsBookingMemberQuery(int BookingId, int UserId) : IRequest<bool>;
