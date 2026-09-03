using StayBook.Domain.Users;

namespace StayBook.Application.Features.Auth.Dtos;

public record UserDto(int Id, string UserName, string Email, UserRole Role);