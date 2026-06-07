using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler(IUserContext userContext, IProjectRepository projectRepository, IUserRepository userRepository) : IRequestHandler<GetAllProjectsQuery, Result<PagedResults<ProjectDto>>>
{
    public async Task<Result<PagedResults<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var isAdmin = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser!.Id), UserRole.Admin);
        var isLeader = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Leader);

        if (!isAdmin && !isLeader)
        {
            return Result<PagedResults<ProjectDto>>.Failure(
                new List<string> { "You are not allowed to view projects" },
                "Forbidden"
            );
        }
        string? managerIdFilter = isLeader && !isAdmin ? currentUser.Id : null;

        var (projects, totalCount) = await projectRepository.GetAllMatchingAsync(
            request.SearchPhrase!,
            request.PageNumber,
            request.PageSize,
            managerIdFilter
        );

        foreach (var p in projects)
        {
            Console.WriteLine(
                p.RowVersion == null
                    ? "NULL"
                    : Convert.ToBase64String(p.RowVersion)
            );
        }
        var projectsDto = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description!,
            ManagerId = p.ManagerId,
            ManagerName = p.Manager.UserName!,

            RowVersion = Convert.ToBase64String(p.RowVersion),
            ProjectStatus = p.Tasks.Any() &&
             p.Tasks.All(t => t.Status == TaskItemStatus.Done)
        ? ProjectStatus.Done
        : ProjectStatus.Pending
        }).ToList();

        var pagedResults = new PagedResults<ProjectDto>(projectsDto, totalCount, request.PageNumber, request.PageSize);

        return Result<PagedResults<ProjectDto>>.Success(pagedResults, "Projects retrieved successfully");

    }
}
