using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Repositories;

public class ProjectRepository(AppDbContext dbContext) : IProjectRepository
{
    public async Task<Guid> Create(Project entity)
    {
        dbContext.Projects.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }
    public async Task<int> CountAllProjectsAsync()
    {
        return await dbContext.Projects.CountAsync();
    }
    public async Task<(IEnumerable<Project> Projects, int TotalCount)> GetAllMatchingAsync(string searchPhrase, int pageNumber, int pageSize, string? managerId = null)
    {
        var query = dbContext.Projects.Include(p => p.Manager).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchPhrase))
            query = query.Where(p => p.Name.Contains(searchPhrase));

        if (!string.IsNullOrWhiteSpace(managerId))
            query = query.Where(p => p.ManagerId == managerId);

        var totalCount = await query.CountAsync();
        var projects = await query.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        return (projects, totalCount);
    }
    public async Task<string> GetProjectManagerIdAsync(Guid projectId)
    {
        var managerId = await dbContext.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.ManagerId)
            .FirstOrDefaultAsync();
        return managerId!;
    }
    public async Task<int> CountByManagerIdAsync(string managerId)
    {
        return await dbContext.Projects
            .Where(p => p.ManagerId == managerId)
            .CountAsync();
    }
    public async Task<Project?> GetByIdAsync(Guid id)
    {
        var projects = await dbContext.Projects
            .Include(r => r.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id);
        return projects;
    }
    public Task SaveChanges()
        => dbContext.SaveChangesAsync();

}