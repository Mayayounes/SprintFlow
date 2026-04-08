using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface IProjectRepository
{
    IQueryable<Project> GetAll();
    Task<Project?> GetByIdAsync(Guid id);
    Task<Guid> Create(Project entity);
    Task SaveChanges();
    Task<(IEnumerable<Project> Projects, int TotalCount)> GetAllMatchingAsync(
    string searchPhrase, int pageNumber, int pageSize, string? managerId = null);
    Task<string> GetProjectManagerIdAsync(Guid projectId);

}
