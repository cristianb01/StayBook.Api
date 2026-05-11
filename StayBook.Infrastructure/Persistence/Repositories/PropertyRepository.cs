using Microsoft.EntityFrameworkCore;
using StayBook.Application.Interfaces;
using StayBook.Domain.Enums;
using StayBook.Domain.Properties;

namespace StayBook.Infrastructure.Persistence.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _context;

    public PropertyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Property>> GetAvailableProperties(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return _context.Properties
            .Where (p => !_context.Bookings.Any(b => 
                b.PropertyId == p.Id
                && (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed)
                && b.DateRange.StartDate < endDate 
                && b.DateRange.EndDate > startDate))
            .ToListAsync(cancellationToken);
    }
}