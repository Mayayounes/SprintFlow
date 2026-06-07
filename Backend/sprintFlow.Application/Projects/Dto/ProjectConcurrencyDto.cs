namespace sprintFlow.Application.Projects.Dto;

public class ProjectConcurrencyDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string RowVersion { get; set; } = default!;
}