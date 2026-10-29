using StayBook.Domain.Users;

namespace StayBook.Domain.Auth;

public class RefreshToken
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string TokenHash { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }

    public RefreshToken(int userId, string tokenHash, DateTime expiresAtUtc)
    {
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
    }

    public void Revoke(DateTime revokeTime)
    {
        RevokedAtUtc = revokeTime;
    }
}