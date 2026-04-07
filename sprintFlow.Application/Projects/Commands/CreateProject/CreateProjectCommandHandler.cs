using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler(IUserContext userContext, IMapper mapper, IProjectRepository projectRepository) : IRequestHandler<CreateProjectCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var project = mapper.Map<Project>(request);
        project.ManagerId = currentUser.Id;
        Guid id = await projectRepository.Create(project);
        return Result<Guid>.Success(id, "Project created successfully");

    }
}
