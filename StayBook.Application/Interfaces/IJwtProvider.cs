using StayBook.Domain.Users;

namespace StayBook.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateJwtToken(User user);
}