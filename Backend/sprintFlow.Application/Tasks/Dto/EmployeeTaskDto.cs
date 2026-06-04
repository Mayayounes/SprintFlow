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
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public long? DurationInSeconds { get; set; }
    public DateTime? StartedAtLocal { get; set; }
    public DateTime? CompletedAtLocal { get; set; }

}
