using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager , AppDbContext dbContext , RoleManager<IdentityRole> roleManager) : IUserRepository
{
    public Task<User?> FindByEmailAsync(string email)
            => userManager.FindByEmailAsync(email);

    public Task<IdentityResult> CreateAsync(User user, string password)
            => userManager.CreateAsync(user, password);

    public Task<IdentityResult> AddToRoleAsync(User user, string role)
            => userManager.AddToRoleAsync(user, role);

    public async Task<(IEnumerable<User>, int)> GetAllMatchingAsync(string? role, int pageNumber, int pageSize)
    {
        var query = dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(role))
        {
            query = from user in dbContext.Users
                    join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
                    join roleTable in dbContext.Roles on userRole.RoleId equals roleTable.Id
                    where roleTable.Name == role
                    select user;
        }
        var count = await query.CountAsync();
        var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
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

}
