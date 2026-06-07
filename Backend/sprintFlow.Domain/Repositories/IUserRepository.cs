using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
namespace sprintFlow.Domain.Repositories;

public interface IUserRepository
{   
    Task<(IEnumerable<User>, int)> GetAllMatchingAsync(string? searchRole, int pageNumber, int pageSize);
    Task<bool> IsUserInRoleAsync(Guid userId, UserRole role);
    Task<Dictionary<string, int>> CountUsersByRoleAsync();
    Task<int> CountEmployeeTasksAsync(Guid userId);
    Task<int> CountLeaderProjectsAsync(Guid userId);
    Task<bool> DeleteUserAsync(User user);
    Task<User?> GetByIdAsync(string userId);
    Task<(bool success, User? user, string? error)> UpdateUserAsync(
        User user,
        byte[] submittedRowVersion);
}
