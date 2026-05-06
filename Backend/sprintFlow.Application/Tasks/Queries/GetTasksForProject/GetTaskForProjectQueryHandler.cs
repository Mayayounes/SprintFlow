using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetTasksForProject;

public class GetTaskForProjectQueryHandler(IMapper mapper ,IUserContext userContext , ITaskRepository taskRepository ,IUserRepository userRepository) : IRequestHandler<GetTaskForProjectQuery, Result<PagedResults<TaskItemDto>>>
{
    public async Task<Result<PagedResults<TaskItemDto>>> Handle(GetTaskForProjectQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var isAdmin = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser!.Id), UserRole.Admin);
        var isLeader = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Leader);

        if (!isAdmin && !isLeader)
        {
            return Result<PagedResults<TaskItemDto>>.Failure(
                new List<string> { "You are not allowed to view Tasks" },
                "Forbidden"
            );
        }
        string? managerIdFilter = isLeader && !isAdmin ? currentUser.Id : null;

        var (tasks, totalCount) = await taskRepository.GetAllMatchingAsync(
            request.ProjectId,
            request.SearchTask,
            request.PageNumber,
            request.PageSize
        );

        var tasksDto = mapper.Map<List<TaskItemDto>>(tasks);

        var result = new PagedResults<TaskItemDto>(
            tasksDto,
            totalCount,
            request.PageNumber,
            request.PageSize
        );

        return Result<PagedResults<TaskItemDto>>.Success(
            result,
            "Tasks retrieved successfully"
        );
    }
}