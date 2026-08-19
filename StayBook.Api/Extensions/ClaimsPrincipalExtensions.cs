using System.Security.Claims;

namespace StayBook.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetCurrentUserId(this ClaimsPrincipal user, out int userId)
    {
        var claimValue = user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claimValue, out userId);
    }
}
