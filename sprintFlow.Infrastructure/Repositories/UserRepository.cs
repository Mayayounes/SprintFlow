using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _AppDbContext;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
    {
        _userManager = userManager;
        _AppDbContext = appDbContext;
        _roleManager = roleManager;
    }

    public Task<User?> FindByEmailAsync(string email)
            => _userManager.FindByEmailAsync(email);

    public Task<IdentityResult> CreateAsync(User user, string password)
            => _userManager.CreateAsync(user, password);

    public Task<IdentityResult> AddToRoleAsync(User user, string role)
            => _userManager.AddToRoleAsync(user, role);

    public async Task<(IEnumerable<User>, int)> GetAllMatchingAsync(string? role, int pageNumber, int pageSize)
    {
        var query = _AppDbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(role))
        {
            query = from user in _AppDbContext.Users
                    join userRole in _AppDbContext.UserRoles on user.Id equals userRole.UserId
                    join roleTable in _AppDbContext.Roles on userRole.RoleId equals roleTable.Id
                    where roleTable.Name == role
                    select user;
        }
        var count = await query.CountAsync();
        var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return (users, count);
    }
}
