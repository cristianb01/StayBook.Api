using System.Security.Authentication;
using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Auth.Commands;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Interfaces;
using StayBook.Domain.Auth;
using StayBook.Infrastructure.Helpers;

namespace StayBook.Application.Features.Auth.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    
    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmail(request.Email, cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }
        
        var passwordValid = _passwordHasher.Verify(user.PasswordHash, request.Password);
        if (!passwordValid)
        {
            throw new InvalidCredentialsException();
        }
        
        var tokenResponse = _jwtProvider.GenerateJwtToken(user);

        var refreshToken = RefreshTokenGenerator.GenerateToken();
        
        var refreshTokenEntity = new RefreshToken(
            user.Id,
            RefreshTokenGenerator.HashToken(refreshToken),
            DateTime.UtcNow.AddDays(7)
        );
        
        await  _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        
        return new LoginResult(
            tokenResponse.AccessToken,
            refreshToken,
            tokenResponse.ExpiresAtUtc,
            refreshTokenEntity.ExpiresAtUtc
        );
    }
}