using StayBook.Application.Interfaces;

namespace StayBook.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) 
        =>  _context.SaveChangesAsync(cancellationToken); 
}