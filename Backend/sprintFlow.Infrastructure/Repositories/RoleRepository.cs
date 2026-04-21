using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Repositories;

public class RoleRepository(RoleManager<IdentityRole> roleManager , AppDbContext dbContext) : IRoleRepository
{
    public async Task<List<string>> GetAllRolesAsync()
    {
        return await dbContext.Roles
            .Select(r => r.Name!)
            .ToListAsync();
    }
    public Task<bool> RoleExistsAsync(string roleName)
        => roleManager.RoleExistsAsync(roleName);
    public async Task<Dictionary<string, string?>> GetRolesForUsersAsync(List<string> userIds)
    {
        var userRoles = await dbContext.UserRoles
            .Where(ur => userIds.Contains(ur.UserId))
            .Join(dbContext.Roles,
                  ur => ur.RoleId,
                  r => r.Id,
                  (ur, r) => new
                  {
                      ur.UserId,
                      RoleName = r.Name
                  })
            .ToListAsync();

        return userRoles
            .GroupBy(x => x.UserId)
            .ToDictionary(
                g => g.Key,
                g => g.First().RoleName
            );
    }
}
