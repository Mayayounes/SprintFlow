using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<Result<Guid>>
{    
    public Guid TaskId { get; set; }
    public TaskItemStatus Status { get; set; }
}
