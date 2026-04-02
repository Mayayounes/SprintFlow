namespace sprintFlow.Domain.Entities;

public class Tasks
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }

    //"assigned to" [Fk to users table]

    public Guid ProjectID { get; set; }
}
