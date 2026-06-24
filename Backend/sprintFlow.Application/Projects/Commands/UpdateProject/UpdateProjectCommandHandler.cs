using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IMapper mapper,IUserContext userContext, IProjectRepository projectRepository , IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if (project == null)
        {
            return Result<ProjectDto>.Failure(
                new List<string> { "Project not found" },
                "Not Found"
            );
        }
        var currentUser = userContext.GetCurrentUser();
        if (project.ManagerId != currentUser!.Id)
        {
            return Result<ProjectDto>.Failure(
                new List<string> { "You are not allowed to update this project" },
                "Forbidden"
);
        }
        var submittedVersion =Convert.FromBase64String(request.RowVersion);

        await projectRepository.SetOriginalRowVersion(project,submittedVersion);

        project.Name = request.Name;
        project.Description = request.Description;

        await unitOfWork.SaveChangesAsync();

        var dto = mapper.Map<ProjectDto>(project);

        return Result<ProjectDto>.Success(dto, "Project updated successfully"); ;
        }
}