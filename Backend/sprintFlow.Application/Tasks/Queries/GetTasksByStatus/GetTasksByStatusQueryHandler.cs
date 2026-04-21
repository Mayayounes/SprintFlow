using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;
using System.Xml;

namespace sprintFlow.Application.Tasks.Queries.GetTasksByStatus;

public class GetTasksByStatusQueryHandler(IUserContext userContext, IProjectRepository projectRepository, IMapper mapper) : IRequestHandler<GetTasksByStatusQuery, Result<PagedResults<TaskItemDto>>>
{
    public async Task<Result<PagedResults<TaskItemDto>>> Handle(GetTasksByStatusQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null)
        {
            return Result<PagedResults<TaskItemDto>>.Failure(
                new List<string> { "Project not found." }
            );
        }

        var currentUser = userContext.GetCurrentUser();
        var managerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);

        if (currentUser.Id != managerId)
        {
            return Result<PagedResults<TaskItemDto>>.Failure(
                new List<string> { "You are not authorized to view this Task" }
            );
        }

        var query = project.Tasks.AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(t => t.Status == request.Status.Value);
        }

        var totalCount = query.Count();

        var taskList = query.ToList();

        var dtoList = mapper.Map<List<TaskItemDto>>(taskList);

        var pagedResult = new PagedResults<TaskItemDto>(
            dtoList,
            totalCount,
            pageNumber: 1,
            pageSize: totalCount == 0 ? 1 : totalCount
        );

        // 7. Return
        return Result<PagedResults<TaskItemDto>>.Success(pagedResult);
    }
}
