using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace sprintFlow.Infrastructure.Repositories;

public class ProjectRepository(AppDbContext dbContext) : IProjectRepository
{
    public async Task<Guid> Create(Project entity)
    {
        dbContext.Projects.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var projects = await dbContext.Projects
            .Include(r => r.Tasks)
            .ToListAsync();
        return projects;
    }
    public async Task<(IEnumerable<Project>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber ,int pageSize)
    {
        var baseQuery = dbContext.Projects
            .Where(r => searchPhrase == null || (r.Name.Contains(searchPhrase) || r.Description.Contains(searchPhrase)));


        var totalCount = await baseQuery.CountAsync();

        var projects = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (projects, totalCount);
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