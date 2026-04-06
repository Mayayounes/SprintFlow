using MediatR;
using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Tasks.Queries.GetByIdForProject;

public class GetByIdForProjectQuery(Guid projectId , Guid taskId) : IRequest<TaskItemDto>
{
    public Guid ProjectId { get; set; } = projectId;
    public Guid TaskId { get; set; } = taskId;
}
