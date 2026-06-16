using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommand : IRequest<Result<TaskItemDto>>
{
    public Guid TaskId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateOnly Deadline { get; set; }
    public Guid ProjectId { get; set; }
    public string RowVersion { get; set; } = default!;

}
