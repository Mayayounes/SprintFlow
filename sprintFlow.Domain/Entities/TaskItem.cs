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

    //EmployeeID reference to user
    public User Employee { get; set; } = default!;
    public string EmployeeId { get; set; } = default!;
    //ProjectID reference to project
    public Project Project { get; set; } = default!;
    public Guid ProjectId { get; set; }

}
