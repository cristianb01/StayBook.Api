using MediatR;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Models;

namespace StayBook.Application.Features.Bookings.Queries;

public record GetAllBookingsQuery(PaginationFilters Filters): IRequest<List<BookingDto>>
{
}