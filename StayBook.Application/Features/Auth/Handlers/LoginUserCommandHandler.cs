using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Auth.Commands;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Auth.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    
    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmail(request.Email, cancellationToken);

        if (user == null)
        {
            throw new ResourceNotFoundException("User not found");
        }
        
        var passwordValid = _passwordHasher.Verify(user.PasswordHash, request.Password);
        if (!passwordValid)
        {
            throw new InvalidPasswordException();
        }
        
        var tokenResponse = _jwtProvider.GenerateJwtToken(user);
        return new LoginResponse(tokenResponse.AccessToken, tokenResponse.ExpiresAtUtc);
    }
}