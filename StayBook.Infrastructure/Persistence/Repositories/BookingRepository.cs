using Microsoft.EntityFrameworkCore;
using StayBook.Application.Interfaces;
using StayBook.Application.Models;
using StayBook.Domain.Bookings;
using StayBook.Domain.Enums;

namespace StayBook.Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _dbContext;

    public BookingRepository(AppDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> HasOverlapAsync(int propertyId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return _dbContext.Bookings.AnyAsync(
            b => b.PropertyId == propertyId
            && (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed)
            && b.DateRange.StartDate < endDate
            && b.DateRange.EndDate > startDate,
            cancellationToken);
    }

    public Task<List<Booking>> GetAllAsync(PaginationFilters paginationFilters, CancellationToken cancellationToken)
    {
        return _dbContext.Bookings
            .OrderByDescending(b => b.CreatedAt)
            .Skip(paginationFilters.Skip)
            .Take(paginationFilters.Take)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Booking?> GetByIdAsync(int bookingId, CancellationToken cancellationToken)
    {
        return _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
    }

    public Task<Booking?> GetByIdWithPropertyAndConversationAsync(int bookingId, CancellationToken cancellationToken)
    {
        return _dbContext.Bookings
            .Include(b => b.Property)
            .Include(b => b.Conversation)
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
    }

    public Task<Booking?> GetForConversationAsync(int bookingId, CancellationToken cancellationToken)
    {
        return _dbContext.Bookings
            .Include(b => b.Guest)
            .Include(b => b.Host)
            .Include(b => b.Conversation)
                .ThenInclude(c => c!.Messages)
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
    }

    public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken)
    {
        _dbContext.Bookings.Update(booking);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}