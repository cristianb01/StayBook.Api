using System.Security.Claims;
using StayBook.Exceptions;

namespace StayBook.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int TryGetCurrentUserId(this ClaimsPrincipal user)
    {
        var claimValue = user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (claimValue is null || !int.TryParse(claimValue, out var userId))
            throw new UnauthorizedException("User Id claim is missing.");

        return userId;
    }
}
