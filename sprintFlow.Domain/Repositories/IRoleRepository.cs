namespace sprintFlow.Domain.Repositories;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);

}
