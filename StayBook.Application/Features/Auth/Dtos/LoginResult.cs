namespace StayBook.Application.Features.Auth.Dtos;

public record LoginResult(
        string AccessToken, 
        string RefreshToken, 
        DateTime AccessTokenExpiresAtUtc,
        DateTime RefreshTokenExpiresAtUtc
    );