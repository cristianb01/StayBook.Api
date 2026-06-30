using StayBook.Domain.Users;

namespace StayBook.Application.Interfaces;

public interface IUserRepository
{
    Task<User> Add(User user, CancellationToken cancellationToken);
    Task<User?> FindByEmail(string email, CancellationToken cancellationToken);
}