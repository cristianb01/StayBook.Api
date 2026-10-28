using System.Security.Cryptography;
using System.Text;

namespace StayBook.Infrastructure.Helpers;

public static class RefreshTokenGenerator
{
    public static string GenerateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}