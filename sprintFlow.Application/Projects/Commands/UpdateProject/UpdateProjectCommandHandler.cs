using AutoMapper;
using MediatR;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IMapper mapper,IUserContext userContext ,IProjectRepository projectRepository) : IRequestHandler<UpdateProjectCommand>
{
    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if (project == null)
            throw new NotFoundException(nameof(Project), request.Id.ToString());
        var currentUser = userContext.GetCurrentUser();
        if (project.ManagerId != currentUser.Id)
        {
            throw new NotAuthorizedException("User", "update this project");
        }
        mapper.Map(request, project);
        await projectRepository.SaveChanges();
    }
}
