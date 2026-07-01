using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace sprintFlow.Application.Users;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;
        if (user == null)
            throw new InvalidOperationException("User Context is not present");
        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return null;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return null;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
        var timezone =user.FindFirst("timezone")?.Value ?? "UTC";
        return new CurrentUser(userId, email, roles, timezone);

    }
}
