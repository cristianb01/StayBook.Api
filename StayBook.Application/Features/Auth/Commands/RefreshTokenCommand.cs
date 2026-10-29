using StayBook.Application.Features.Auth.Dtos;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : ICommand<LoginResult>;