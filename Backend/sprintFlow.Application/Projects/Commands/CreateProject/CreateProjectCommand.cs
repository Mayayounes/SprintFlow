using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
