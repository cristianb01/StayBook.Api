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
}