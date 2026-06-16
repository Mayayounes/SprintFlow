using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Projects.Dto;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ManagerId { get; set; } = default!;
    public string ManagerName { get; set; } = default!;
    public ProjectStatus ProjectStatus { get; set; }
    public string RowVersion { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}