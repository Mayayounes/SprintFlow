namespace sprintFlow.Application.Tasks.Dto;

public class TaskConcurrencyDto
{
    public Guid TaskId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateOnly AssignedDate { get; set; }

    public DateOnly Deadline { get; set; }
    public Guid ProjectId { get; set; }
    public string RowVersion { get; set; } = default!;
}
