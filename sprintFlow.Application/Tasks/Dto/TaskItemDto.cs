using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Tasks.Dto;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }
    public string EmployeeId { get; set; } = default!;

}
