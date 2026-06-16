using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface ITaskRepository
{
    Task<Guid> Create(TaskItem entity);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<(IEnumerable<TaskItem>, int)> GetAllMatchingAsync(Guid projectId,string? searchTask,int pageNumber,int pageSize);
    Task<(IEnumerable<TaskItem>, int)> GetMyTasksAsync(string userId, int pageNumber, int pageSize , string? status);
    Task<List<TaskItem>> GetAssignedTasksWithEmployeesAsync();
    Task<List<TaskItem>> GetActiveAssignedTasksAsync();
    Task<(bool Success, TaskItem? Latest)> SaveChangesSafe();
    Task SetOriginalRowVersion(TaskItem task, byte[] rowVersion);
}
