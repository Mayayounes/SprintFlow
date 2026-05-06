using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Tasks.Dto;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }
    public string? EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = default!;
    public string? EmployeeName { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Duration { get; set; }
    public TaskCompletionStatus CompletionStatus { get; set; } = TaskCompletionStatus.NotCompleted;
    public DateTime? StartedAtLocal { get; set; }
    public DateTime? CompletedAtLocal { get; set; }
}
