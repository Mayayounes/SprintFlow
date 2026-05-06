using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetMyTasks;

public class GetMyTasksQueryHandler(IMapper mapper ,ITaskRepository taskRepository, IUserContext userContext) : IRequestHandler<GetMyTasksQuery, Result<PagedResults<EmployeeTaskDto>>>
{
    public async Task<Result<PagedResults<EmployeeTaskDto>>> Handle(GetMyTasksQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var (tasks, totalCount) = await taskRepository.GetMyTasksAsync(currentUser!.Id,request.PageNumber,request.PageSize, request.Status);

        var items = mapper.Map<List<EmployeeTaskDto>>(tasks);

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
