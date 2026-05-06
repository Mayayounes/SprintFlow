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

        //var tasksDto = mapper.Map<List<TaskItemDto>>(tasks);

        //var userTimeZone = currentUser!.TimeZoneId;
        //foreach (var task in tasksDto)
        //{
        //    if (task.StartedAt != null)
        //        task.StartedAtLocal = TimeZoneHelper.ToUserTime(task.StartedAt.Value, userTimeZone);

        //    if (task.CompletedAt != null)
        //        task.CompletedAtLocal = TimeZoneHelper.ToUserTime(task.CompletedAt.Value, userTimeZone);
        //}

        var userTimeZone = currentUser!.TimeZoneId;

        var tasksDto = tasks.Select(task => new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            AssignedDate = task.AssignedDate,
            Deadline = task.Deadline,
            EmployeeId = task.EmployeeId,
            ProjectId = task.ProjectId,
            ProjectName = task.Project?.Name,
            EmployeeName = task.Employee?.UserName,

            StartedAt = task.StartedAt,
            CompletedAt = task.CompletedAt,

            StartedAtLocal = task.StartedAt == null
                ? null
                : TimeZoneHelper.ToUserTime(task.StartedAt.Value, userTimeZone),

            CompletedAtLocal = task.CompletedAt == null
                ? null
                : TimeZoneHelper.ToUserTime(task.CompletedAt.Value, userTimeZone),

            Duration = task.Duration,
            CompletionStatus = task.CompletionStatus
        }).ToList();

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