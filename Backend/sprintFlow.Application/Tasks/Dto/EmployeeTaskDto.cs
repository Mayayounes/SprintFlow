namespace sprintFlow.Application.Tasks.Dto;

public class EmployeeTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = default!;
    public string? ManagerName { get; set; } = default!;
}
