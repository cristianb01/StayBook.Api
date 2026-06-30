using StayBook.Domain.Users;
using StayBook.Infrastructure.Models;

namespace StayBook.Application.Interfaces;

public interface IJwtProvider
{
    TokenResponse GenerateJwtToken(User user);
}