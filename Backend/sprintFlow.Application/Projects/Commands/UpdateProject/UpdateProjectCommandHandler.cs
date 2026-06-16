using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IUserContext userContext, IProjectRepository projectRepository) : IRequestHandler<UpdateProjectCommand, Result<ProjectConcurrencyDto>>
{
    public async Task<Result<ProjectConcurrencyDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if (project == null)
        {
            return Result<ProjectConcurrencyDto>.Failure(
                new List<string> { "Project not found" },
                "Not Found"
            );
        }
        var currentUser = userContext.GetCurrentUser();
        if (project.ManagerId != currentUser!.Id)
        {
            return Result<ProjectConcurrencyDto>.Failure(
                new List<string> { "You are not allowed to update this project" },
                "Forbidden"
);
        }
        var submittedVersion =Convert.FromBase64String(request.RowVersion);
        await projectRepository.SetOriginalRowVersion(project,submittedVersion);

        project.Name = request.Name;
        project.Description = request.Description;

        var result = await projectRepository.SaveChangesSafe();

        if (!result.Success)
        {
            return Result<ProjectConcurrencyDto>.Failure(
                ["ConcurrencyConflict"],
                "ConcurrencyConflict",
                result.Latest == null
                    ? null
                    : new ProjectConcurrencyDto
                    {
                        Id = result.Latest.Id,
                        Name = result.Latest.Name,
                        Description = result.Latest.Description ?? "",
                        RowVersion = Convert.ToBase64String(
                            result.Latest.RowVersion)
                    });
        }

        return Result<ProjectConcurrencyDto>.Success(
            new ProjectConcurrencyDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                RowVersion = Convert.ToBase64String(
                    project.RowVersion)
            },
            "Project updated successfully");
    }
}