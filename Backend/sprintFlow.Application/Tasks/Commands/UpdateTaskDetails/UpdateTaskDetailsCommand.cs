using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommand : IRequest<Result<Guid>>
{
    public Guid TaskId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Status { get; set; } = "ToDo";
    //public DateOnly AssignedDate { get; set; }
    public DateOnly Deadline { get; set; }
    public Guid ProjectId { get; set; }
}
