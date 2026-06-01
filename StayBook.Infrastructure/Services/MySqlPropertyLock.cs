using Microsoft.EntityFrameworkCore;
using StayBook.Application.Interfaces;
using StayBook.Infrastructure.Persistence;

namespace StayBook.Infrastructure.Services;

public class MySqlPropertyLock : IMySqlPropertyLock
{
    private readonly AppDbContext _dbContext;

    public MySqlPropertyLock(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task ExecuteAsync(int propertyId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Properties.FromSqlInterpolated(
            $"""
                SELECT Id, HostId, Name, Description
                FROM Properties
                WHERE Id = {propertyId}
                FOR UPDATE
             """)
            .FirstOrDefaultAsync(cancellationToken);
    }
}