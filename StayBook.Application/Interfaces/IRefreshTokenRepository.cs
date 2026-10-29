using StayBook.Domain.Auth;

namespace StayBook.Application.Interfaces;

public interface IRefreshTokenRepository
{
    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    public Task<RefreshToken?> GetByHashForUpdateAsync(string tokenHash, CancellationToken cancellationToken);
}