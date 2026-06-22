using StayBook.Domain.Users;

namespace StayBook.Api.Models;

public record RegisterUserRequest(string  Email, string Password, string UserName, UserRole Role);