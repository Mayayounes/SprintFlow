using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Tasks.Commands.AssignEmployeeToTask;
using sprintFlow.Application.Tasks.Commands.CreateTask;
using sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;
using sprintFlow.Application.Tasks.Commands.UpdateTaskStatus;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Tasks.Queries.GetByIdForProject;
using sprintFlow.Application.Tasks.Queries.GetTasksByStatus;
using sprintFlow.Application.Tasks.Queries.GetTasksForProject;
using sprintFlow.Domain.Constants;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class TaskController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<ActionResult> CreateTask([FromRoute] Guid projectId,[FromBody]CreateTaskCommand command)
    {
        command.ProjectId = projectId;
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetByIdForProject), new { projectId = projectId, taskId = result.Data }, result);
    }
    [HttpGet]
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Leader))]
    public async Task<ActionResult> GetAllForProject([FromRoute] Guid projectId,[FromQuery] int pageNumber ,[FromQuery] int pageSize ,[FromQuery] string? searchTask = null)
    {
        var query = new GetTaskForProjectQuery(projectId)
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTask = searchTask
        };

        var result = await mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpGet("{taskId}")]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<ActionResult<TaskItemDto>> GetByIdForProject([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        var task = await mediator.Send(new GetByIdForProjectQuery(projectId, taskId));
        if (task == null)
            return NotFound();
        return Ok(task);
    }
    [HttpPatch("{taskId}/updateStatus")]
    [Authorize(Roles =nameof(UserRole.Employee))]
    public async Task<ActionResult> UpdatetaskStatus([FromRoute] Guid projectId, [FromRoute]Guid taskId ,[FromBody]UpdateTaskStatusCommand command)
    {
        command.TaskId = taskId;
        var result = await mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);

    }
    [HttpPatch("{taskId}/update")]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<ActionResult> UpdatetaskDetails([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] UpdateTaskDetailsCommand command)
    {
        command.TaskId = taskId;
        command.ProjectId = projectId;
        var result = await mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);

    }
    [HttpPost("{taskId}/assignEmployee")]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<IActionResult> AssignEmployeeToTask([FromRoute] Guid projectId, [FromRoute] Guid taskId,AssignEmployeeToTaskCommand command)
    {
        command.TaskId = taskId;
        command.ProjectId = projectId;
        var result = await mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
    [HttpGet("filter")]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasksByStatus(
        [FromRoute] Guid projectId,
        [FromQuery] TaskItemStatus status)
    {
        var result = await mediator.Send(new GetTasksByStatusQuery(projectId, status));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
