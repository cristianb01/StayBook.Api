using StayBook.Application.Models;
using StayBook.Domain.Bookings;

namespace StayBook.Application.Interfaces;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task<bool> HasOverlapAsync(int propertyId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<List<Booking>> GetAllAsync(PaginationFilters paginationFilters, CancellationToken cancellationToken);
    Task<Booking?> GetByIdAsync(int bookingId, CancellationToken cancellationToken);
    Task<Booking?> GetByIdWithPropertyAsync(int bookingId, CancellationToken cancellationToken);
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken);
}