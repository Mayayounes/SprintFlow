using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<bool> RoleExistsAsync(string roleName)
        => _roleManager.RoleExistsAsync(roleName);
}
