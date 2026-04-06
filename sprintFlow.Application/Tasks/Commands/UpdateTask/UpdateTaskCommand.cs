using MediatR;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest
{    
    public Guid TaskId { get; set; }
    public TaskItemStatus Status { get; set; }
}
