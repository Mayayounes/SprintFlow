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

        var items = tasks.Select(t => new EmployeeTaskDto
        {
            Id = t.Id,
            Title = t.Title!,
            Description = t.Description!,
            Status = t.Status.ToString(),
            AssignedDate = t.AssignedDate,
            Deadline = t.Deadline,
            ProjectId = t.ProjectId,
            ProjectName = t.Project.Name,
            ManagerName = t.Project.Manager != null ? t.Project.Manager.UserName : null,
            StartedAt = t.StartedAt,
            CompletedAt = t.CompletedAt,
            Duration = (t.StartedAt != null && t.CompletedAt != null)? (t.CompletedAt - t.StartedAt)?.ToString(@"hh\:mm\:ss"): null
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
