namespace sprintFlow.Domain.Repositories;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<Dictionary<string, string?>> GetRolesForUsersAsync(List<string> userIds);
    Task<List<string>> GetAllRolesAsync();

}
