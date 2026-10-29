using Microsoft.EntityFrameworkCore;
using StayBook.Application.Interfaces;
using StayBook.Domain.Auth;

namespace StayBook.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _appDbContext;

    public RefreshTokenRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _appDbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<RefreshToken?> GetByHashForUpdateAsync(string tokenHash, CancellationToken cancellationToken)
    {
        return _appDbContext.RefreshTokens
            .FromSqlInterpolated(
                $"""
                 SELECT Id, UserId, TokenHash, ExpiresAtUtc, RevokedAtUtc
                 FROM RefreshTokens
                 WHERE TokenHash = {tokenHash}
                 FOR UPDATE
                 """
            )
            .FirstOrDefaultAsync(cancellationToken);
    }
}