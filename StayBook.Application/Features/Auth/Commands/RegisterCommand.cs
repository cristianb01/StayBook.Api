using MediatR;
using StayBook.Application.Features.Auth.Dtos;
using StayBook.Domain.Users;

namespace StayBook.Application.Features.Auth.Commands;

public record RegisterCommand(string Email, string Password, string UserName, UserRole Role) : IRequest<UserDto>;