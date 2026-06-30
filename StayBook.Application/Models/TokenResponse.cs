namespace StayBook.Infrastructure.Models;

public record TokenResponse(string AccessToken, DateTime ExpiresAtUtc);