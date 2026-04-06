using MediatR;
using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Tasks.Queries.GetTasksForProject;

public class GetTaskForProjectQuery(Guid projectId) : IRequest<IEnumerable<TaskItemDto>>
{
    public Guid ProjectId { get; } = projectId;

}
