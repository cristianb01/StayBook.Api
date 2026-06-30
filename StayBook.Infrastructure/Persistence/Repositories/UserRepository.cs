using Microsoft.EntityFrameworkCore;
using StayBook.Application.Interfaces;
using StayBook.Domain.Users;

namespace StayBook.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Add(User user, CancellationToken cancellationToken)
    {
        var addedUser = (await _dbContext.Users.AddAsync(user, cancellationToken)).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return addedUser;
    }

    public Task<User?> FindByEmail(string email, CancellationToken cancellationToken)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}