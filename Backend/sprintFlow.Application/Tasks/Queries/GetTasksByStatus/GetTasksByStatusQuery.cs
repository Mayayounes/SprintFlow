using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Tasks.Queries.GetTasksByStatus;

public class GetTasksByStatusQuery(Guid projectId , TaskItemStatus status) : IRequest<Result<PagedResults<TaskItemDto>>>
{
    public Guid ProjectId { get; set; } = projectId;
    public TaskItemStatus? Status { get; set; } = status;
}
