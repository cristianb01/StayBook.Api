using AutoMapper;
using MediatR;
using StayBook.Application.Features.Auth.Commands;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Interfaces;
using StayBook.Domain.Users;

namespace StayBook.Application.Features.Auth.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(request.UserName, passwordHash, request.Email, request.Role);
        
        var addedUser = await _userRepository.Add(user, cancellationToken);
        
        return _mapper.Map<UserDto>(addedUser);
    }
}