using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest<Result<Guid>>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateOnly? AssignedDate { get; set; } = null;
    public DateOnly? Deadline { get; set; } = null;
    public Guid ProjectId { get; set; }

}
