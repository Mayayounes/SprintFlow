using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Tasks.Commands.AssignEmployeeToTask;

public class AssignEmployeeToTaskCommand : IRequest<Result<string>>
{
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TaskId { get; set; }
}
