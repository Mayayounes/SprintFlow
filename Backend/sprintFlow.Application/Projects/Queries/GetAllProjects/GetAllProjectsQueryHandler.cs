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

public class GetAllProjectsQueryHandler(IUserContext userContext, IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetAllProjectsQuery, Result<PagedResults<ProjectDto>>>
{
    public async Task<Result<PagedResults<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var isAdmin = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Admin);
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
            request.SearchPhrase,
            request.PageNumber,
            request.PageSize,
            managerIdFilter
        );
        var projectsDto = mapper.Map<IEnumerable<ProjectDto>>(projects);

        var pagedResults = new PagedResults<ProjectDto>(projectsDto, totalCount, request.PageNumber, request.PageSize);

        return Result<PagedResults<ProjectDto>>.Success(pagedResults, "Projects retrieved successfully");

    }
}
