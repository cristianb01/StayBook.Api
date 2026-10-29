using MediatR;
using StayBook.Application.Features.Auth.Commands;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Interfaces;
using StayBook.Domain.Auth;
using StayBook.Infrastructure.Helpers;

namespace StayBook.Application.Features.Auth.Handlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResult>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IJwtProvider jwtProvider)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var tokenHash = RefreshTokenGenerator.HashToken(request.RefreshToken);

        var existingToken = await _refreshTokenRepository.GetByHashForUpdateAsync(tokenHash, cancellationToken);

        if (existingToken is null || existingToken.RevokedAtUtc.HasValue || existingToken.ExpiresAtUtc <= now)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var user = existingToken.User;

        var newRefreshToken = RefreshTokenGenerator.GenerateToken();
        var newRefreshTokenHash = RefreshTokenGenerator.HashToken(newRefreshToken);

        existingToken.Revoke(now);

        var replacementReplacement = new RefreshToken(existingToken.UserId, newRefreshTokenHash, DateTime.UtcNow.AddDays(7));

        await _refreshTokenRepository.AddAsync(replacementReplacement, cancellationToken);

        var accessToken = _jwtProvider.GenerateJwtToken(user);

        return new LoginResult(accessToken.AccessToken, newRefreshToken, accessToken.ExpiresAtUtc,
            replacementReplacement.ExpiresAtUtc);
    }
}