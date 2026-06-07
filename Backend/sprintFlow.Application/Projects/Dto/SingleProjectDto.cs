using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Projects.Dto;

public class SingleProjectDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ManagerId { get; set; } = default!;
    public ICollection<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();
    public ProjectStatus ProjectStatus { get; set; }
    public string RowVersion { get; set; } = default!;

}
