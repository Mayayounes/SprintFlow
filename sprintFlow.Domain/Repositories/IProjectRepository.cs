using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<Guid> Create(Project entity);
    Task SaveChanges();
    Task<(IEnumerable<Project>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber, int pageSize);

}
