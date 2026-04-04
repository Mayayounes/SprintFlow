using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Infrastructure.Repositories;

public class RoleRepository(RoleManager<IdentityRole> roleManager) : IRoleRepository
{
    public Task<bool> RoleExistsAsync(string roleName)
        => roleManager.RoleExistsAsync(roleName);
}
