using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Projects.Dto;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ManagerId { get; set; } = default!;
    public string ManagerName { get; set; } = default!;

}
