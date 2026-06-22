using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId): IRequest<Result<bool>>;