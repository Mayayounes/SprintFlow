using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : IRequest<Result<bool>>;
