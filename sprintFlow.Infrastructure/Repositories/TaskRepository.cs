using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Repositories;
    public class TaskRepository(AppDbContext dbContext) : ITaskRepository
{
    public async Task<Guid> Create(TaskItem entity)
    {
        dbContext.Tasks.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }
    public async Task Delete(IEnumerable<TaskItem> entities)
    {
        dbContext.Tasks.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }
    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await dbContext.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        dbContext.Tasks.Update(task);
        await dbContext.SaveChangesAsync();
    }
}
