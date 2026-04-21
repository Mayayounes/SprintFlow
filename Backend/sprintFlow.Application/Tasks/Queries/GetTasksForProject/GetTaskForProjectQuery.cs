using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Tasks.Queries.GetTasksForProject;

public class GetTaskForProjectQuery(Guid projectId) : IRequest<Result<PagedResults<TaskItemDto>>>
{
    public Guid ProjectId { get; } = projectId;
    public string? SearchTask { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
