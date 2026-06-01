using Microsoft.EntityFrameworkCore.Storage;
using StayBook.Application.Interfaces;

namespace StayBook.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null) return;
        
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException(
                "No active transaction.");
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _transaction.CommitAsync(cancellationToken);

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            return;
        }

        await _transaction.RollbackAsync(cancellationToken);

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) 
        =>  _dbContext.SaveChangesAsync(cancellationToken); 
}