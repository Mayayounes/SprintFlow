using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
