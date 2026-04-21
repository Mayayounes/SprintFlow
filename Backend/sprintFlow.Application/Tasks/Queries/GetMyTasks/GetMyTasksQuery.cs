using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Tasks.Queries.GetMyTasks;
public class GetMyTasksQuery : IRequest<Result<PagedResults<EmployeeTaskDto>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Status { get; set; }

}
