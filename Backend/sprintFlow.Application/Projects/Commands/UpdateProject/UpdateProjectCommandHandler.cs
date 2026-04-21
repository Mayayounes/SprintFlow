using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IMapper mapper, IUserContext userContext, IProjectRepository projectRepository) : IRequestHandler<UpdateProjectCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if (project == null)
        {
            return Result<Guid>.Failure(
                new List<string> { "Project not found" },
                "Not Found"
            );
        }
        var currentUser = userContext.GetCurrentUser();
        if (project.ManagerId != currentUser.Id)
        {
            return Result<Guid>.Failure(
                new List<string> { "You are not allowed to update this project" },
                "Forbidden"
);
        }
        mapper.Map(request, project);
        await projectRepository.SaveChanges();
        return Result<Guid>.Success(project.Id, "Project updated successfully");

    }
}
