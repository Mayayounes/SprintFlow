using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface ITaskRepository
{
    Task<Guid> Create(TaskItem entity);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TaskItem task);
    Task<(IEnumerable<TaskItem>, int)> GetAllMatchingAsync(Guid projectId,string? searchTask,int pageNumber,int pageSize);
    Task<(IEnumerable<TaskItem>, int)> GetMyTasksAsync(string userId, int pageNumber, int pageSize , string? status);
    Task<List<TaskItem>> GetAssignedTasksWithEmployeesAsync();
    Task SetOriginalRowVersion(TaskItem task, byte[] rowVersion);
    Task<TaskItem?> GetDatabaseValues(TaskItem task);
    Task<List<TaskItem>> GetActiveAssignedTasksAsync();

}
