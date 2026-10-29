using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayBook.Api.Models;
using StayBook.Application.Features.Auth.Commands;
using StayBook.Application.Features.Auth.Dtos;

namespace StayBook.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password, request.UserName, request.Role);
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginUserRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        
        LoginResult loginResult = await _mediator.Send(command);
        
        Response.Cookies.Append(
            "refreshToken", 
            loginResult.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/api/v1/auth",
                Expires = loginResult.RefreshTokenExpiresAtUtc
            }
        );
        
        return Ok(new LoginResponse(loginResult.AccessToken, loginResult.RefreshTokenExpiresAtUtc));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken)
            || string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        
        Response.Cookies.Append("refreshToken",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "api/v1/auth",
                Expires = result.RefreshTokenExpiresAtUtc
            });

        return Ok(new LoginResponse(result.AccessToken, result.AccessTokenExpiresAtUtc));
    }
}