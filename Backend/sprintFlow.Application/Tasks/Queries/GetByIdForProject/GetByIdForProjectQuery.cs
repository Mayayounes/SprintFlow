using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Tasks.Queries.GetByIdForProject;

public class GetByIdForProjectQuery(Guid projectId , Guid taskId) : IRequest<Result<TaskItemDto>>
{
    public Guid ProjectId { get; set; } = projectId;
    public Guid TaskId { get; set; } = taskId;
}
