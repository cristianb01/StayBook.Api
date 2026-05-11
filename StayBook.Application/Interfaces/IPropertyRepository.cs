using StayBook.Domain.Properties;

namespace StayBook.Application.Interfaces;

public interface IPropertyRepository
{
    Task<List<Property>> GetAvailableProperties(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
}