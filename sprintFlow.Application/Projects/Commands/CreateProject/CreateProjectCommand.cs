using MediatR;

namespace sprintFlow.Application.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
