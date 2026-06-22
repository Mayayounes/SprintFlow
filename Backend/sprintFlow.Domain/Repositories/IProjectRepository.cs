using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<Guid> Create(Project entity);
    Task<(IEnumerable<Project> Projects, int TotalCount)> GetAllMatchingAsync(string searchPhrase, int pageNumber, int pageSize, string? managerId = null);
    Task<string> GetProjectManagerIdAsync(Guid projectId);
    Task<int> CountAllProjectsAsync();
    Task<int> CountByManagerIdAsync(string managerId);
    Task<List<Project>> GetByManagerIdWithTasksAsync(string managerId);
    Task<List<Project>> GetAllWithTasksAsync();
    Task UpdateStatusAsync(Guid projectId);
    Task SetOriginalRowVersion(Project project, byte[] rowVersion);
    Task<(bool Success, Project? Latest)> SaveChangesSafe();
    Task Delete(Project project);

}
