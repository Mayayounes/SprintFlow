using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest<Result<Guid>>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Status { get; set; } = "ToDo";
    public DateOnly? AssignedDate { get; set; } = null;
    public DateOnly? Deadline { get; set; } = null;
    //public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }

}
