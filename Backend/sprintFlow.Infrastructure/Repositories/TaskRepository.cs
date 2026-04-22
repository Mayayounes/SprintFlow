using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Repositories;
public class TaskRepository(AppDbContext dbContext) : ITaskRepository
{
    public async Task<(IEnumerable<TaskItem>, int)> GetMyTasksAsync(string userId,int pageNumber,int pageSize, string? status)
    {
        var query = dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.EmployeeId == userId)
            .Include(t => t.Project)
                 .ThenInclude(p => p.Manager)
             .Include(t => t.Employee)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => t.Status.ToString() == status);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.AssignedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    public async Task<(IEnumerable<TaskItem>, int)> GetAllMatchingAsync(Guid projectId,string? searchTask,int pageNumber,int pageSize)
    {
        var query = dbContext.Tasks
             .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Project)
            .Include(t => t.Employee)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTask))
        {
            searchTask = searchTask.ToLower();

            query = query.Where(t =>
                t.Title.ToLower().Contains(searchTask) ||
                t.Description.ToLower().Contains(searchTask) ||
                t.Status.ToString().ToLower().Contains(searchTask)
            );
        }

        var count = await query.CountAsync();

        var tasks = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (tasks, count);
    }
    public async Task<Guid> Create(TaskItem entity)
    {
        dbContext.Tasks.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
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
