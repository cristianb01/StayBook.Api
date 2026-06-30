using MediatR;
using StayBook.Application.Features.Auth.Dtos;

namespace StayBook.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) :  IRequest<LoginResponse>;