using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface ITaskRepository
{
    Task<Guid> Create(TaskItem entity);
    Task Delete(IEnumerable<TaskItem> entities);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TaskItem task);
}
