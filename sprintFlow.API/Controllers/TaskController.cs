using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Commands.CreateTask;
using sprintFlow.Application.Tasks.Commands.UpdateTask;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Tasks.Queries.GetByIdForProject;
using sprintFlow.Application.Tasks.Queries.GetTasksForProject;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class TaskController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<ActionResult> CreateTask([FromRoute] Guid projectId, CreateTaskCommand command)
    {
        command.ProjectId = projectId;
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetByIdForProject), new { projectId = projectId, taskId = result.Data }, result);
    }
    [HttpGet]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    [Authorize(Roles = nameof(UserRole.Leader))]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAllForProject([FromRoute] Guid projectId)
    {
        var tasks = await mediator.Send(new GetTaskForProjectQuery(projectId));
        if (!tasks.IsSuccess)
            return BadRequest(tasks);
        return Ok(tasks);
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
    [HttpPatch("{taskId}/update")]
    [Authorize(Roles =nameof(UserRole.Employee))]
    public async Task<ActionResult> Updatetask([FromRoute] Guid projectId, [FromRoute]Guid taskId ,[FromBody]UpdateTaskCommand command)
    {
        command.TaskId = taskId;
        var result = await mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetByIdForProject), new { projectId = projectId, taskId = result.Data }, result);

    }
}
