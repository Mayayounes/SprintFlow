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
    public long? DurationInSeconds =>
    StartedAt != null && CompletedAt != null
        ? (long?)(CompletedAt - StartedAt)?.TotalSeconds
        : null;

    [NotMapped]
    public TaskCompletionStatus CompletionStatus
    {
        get
        {
            if (CompletedAt == null)
                return TaskCompletionStatus.NotCompleted;

            var completedDate = DateOnly.FromDateTime(CompletedAt.Value);

            if (completedDate < Deadline)
                return TaskCompletionStatus.Early;

            if (completedDate == Deadline)
                return TaskCompletionStatus.OnTime;

            return TaskCompletionStatus.Late;
        }
    }
}
