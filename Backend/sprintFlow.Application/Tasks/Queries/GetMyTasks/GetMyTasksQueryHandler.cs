using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetMyTasks;

public class GetMyTasksQueryHandler(ITaskRepository taskRepository, IUserContext userContext) : IRequestHandler<GetMyTasksQuery, Result<PagedResults<EmployeeTaskDto>>>
{
    public async Task<Result<PagedResults<EmployeeTaskDto>>> Handle(GetMyTasksQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var (tasks, totalCount) = await taskRepository.GetMyTasksAsync(currentUser!.Id,request.PageNumber,request.PageSize, request.Status);

        var userTimeZone = currentUser.TimeZoneId;

        var items = tasks.Select(task => new EmployeeTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            AssignedDate = task.AssignedDate,
            Deadline = task.Deadline,
            ProjectId = task.ProjectId,
            ProjectName = task.Project?.Name,
            StartedAt = task.StartedAt,
            CompletedAt = task.CompletedAt,
            StartedAtLocal = task.StartedAt == null
                ? null
                : TimeZoneHelper.ToUserTime(task.StartedAt.Value, userTimeZone),
            CompletedAtLocal = task.CompletedAt == null
                ? null
                : TimeZoneHelper.ToUserTime(task.CompletedAt.Value, userTimeZone),
            Duration = task.Duration,
        }).ToList();

        var result = new PagedResults<EmployeeTaskDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );


        return Result<PagedResults<EmployeeTaskDto>>.Success(
            result,
            "My tasks retrieved successfully"
        );
    }
}
