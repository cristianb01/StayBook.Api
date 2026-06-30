namespace StayBook.Application.Features.Auth.Dtos;

public record LoginResponse(string AccessToken, DateTime ExpiresAtUtc);