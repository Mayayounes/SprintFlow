using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface ITaskRepository
{
    Task<Guid> Create(TaskItem entity);
    Task Delete(IEnumerable<TaskItem> entities);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TaskItem task);
    IQueryable<TaskItem> GetAll();
    Task<(IEnumerable<TaskItem>, int)> GetAllMatchingAsync(Guid projectId,string? searchTask,int pageNumber,int pageSize);
    Task<(IEnumerable<TaskItem>, int)> GetMyTasksAsync(string userId, int pageNumber, int pageSize , string? status);
}
