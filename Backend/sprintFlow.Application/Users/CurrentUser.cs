namespace sprintFlow.Application.Users;

public record CurrentUser(string Id, string Email, IEnumerable<string> Roles , string TimeZoneId)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
