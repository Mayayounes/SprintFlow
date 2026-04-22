using sprintFlow.Domain.Constants;

namespace sprintFlow.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TaskItemStatus Status { get; set; } = default!;
    public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }
    public User Employee { get; set; } = default!;
    public string? EmployeeId { get; set; } = default!;
    public Project Project { get; set; } = default!;
    public Guid ProjectId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
