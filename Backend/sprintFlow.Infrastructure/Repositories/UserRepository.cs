using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;
using System.Data;

namespace sprintFlow.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager , AppDbContext dbContext , RoleManager<IdentityRole> roleManager) : IUserRepository
{
    public async Task<(IEnumerable<User>, int)> GetAllMatchingAsync(string? searchRole, int pageNumber, int pageSize)
    {
        var query =
     from user in dbContext.Users
     join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
     join role in dbContext.Roles on userRole.RoleId equals role.Id
     select new
     {
         User = user,
         RoleName = role.Name,
         Id = user.Id,
     };

        if (!string.IsNullOrEmpty(searchRole))
        {
            query = query.Where(u => u.RoleName == searchRole);
        }

        var count = await query.CountAsync();

        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.User)
            .ToListAsync();

        return (users, count);
    }
    public async Task<bool> IsUserInRoleAsync(Guid userId, UserRole role)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return false;
        var roleName = role.ToString();
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
            return false;
        return await userManager.IsInRoleAsync(user, roleName);
    }
    public async Task<Dictionary<string, int>> CountUsersByRoleAsync()
    {
        return await (
            from user in dbContext.Users
            join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
            join role in dbContext.Roles on userRole.RoleId equals role.Id
            group user by role.Name into g
            select new
            {
                Role = g.Key,
                Count = g.Count()
            }
        )
        .ToDictionaryAsync(x => x.Role, x => x.Count);
    }
    public async Task<int> CountEmployeeTasksAsync(Guid userId)
    {
        return await dbContext.Tasks
            .CountAsync(t => t.EmployeeId == userId.ToString());
    }
    public async Task<int> CountLeaderProjectsAsync(Guid userId)
    {
        return await dbContext.Projects
            .CountAsync(p => p.ManagerId == userId.ToString());
    }
    public async Task<bool> DeleteUserAsync(User user)
    {
        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }

}
