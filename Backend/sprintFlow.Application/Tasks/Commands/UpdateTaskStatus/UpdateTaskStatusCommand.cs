using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommand : IRequest<Result<Guid>>
{    
    public Guid TaskId { get; set; }
    public int? Status { get; set; }
}
