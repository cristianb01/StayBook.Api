namespace StayBook.Api.Models;

public record LoginResponse (string AccessToken, DateTime ExpiresAtUtc);