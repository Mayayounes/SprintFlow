using AutoMapper;
using MediatR;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IMapper mapper, IProjectRepository projectRepository) : IRequestHandler<UpdateProjectCommand>
{
    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if (project == null)
            throw new NotFoundException(nameof(Project), request.Id.ToString());
        mapper.Map(request, project);
        await projectRepository.SaveChanges();
    }
}
