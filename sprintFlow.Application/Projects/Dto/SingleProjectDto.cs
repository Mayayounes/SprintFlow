using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Projects.Dto;

public class SingleProjectDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();

}
