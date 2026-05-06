using sprintFlow.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace sprintFlow.Domain.Entities;
public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }
    public string? EmployeeId { get; set; }
    public User? Employee { get; set; }
    public Project Project { get; set; } = default!;
    public Guid ProjectId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    [NotMapped]
    public string? Duration => (StartedAt != null && CompletedAt != null) ? (CompletedAt - StartedAt)?.ToString(@"hh\:mm\:ss"): null;
    [NotMapped]
    public TaskCompletionStatus CompletionStatus
    {
        get
        {
            if (CompletedAt == null)
                return TaskCompletionStatus.NotCompleted;

            var deadlineDateTime = Deadline.ToDateTime(TimeOnly.MinValue);

            if (CompletedAt.Value < deadlineDateTime)
                return TaskCompletionStatus.Early;

            if (CompletedAt.Value <= deadlineDateTime)
                return TaskCompletionStatus.OnTime;

            return TaskCompletionStatus.Late;
        }
    }
}