using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface IUserRepository
{   
    Task<User?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> AddToRoleAsync(User user, string role);
    Task<(IEnumerable<User>, int)> GetAllMatchingAsync(string? role, int pageNumber, int pageSize);
    Task<bool> IsUserInRoleAsync(Guid userId, UserRole role);
}
