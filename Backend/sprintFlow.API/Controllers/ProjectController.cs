using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Projects.Queries.GetAllProjects;
using sprintFlow.Application.Projects.Queries.GetProjectById;
using sprintFlow.Domain.Constants;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Policies.AdminOrLeader)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects([FromQuery] GetAllProjectsQuery query)
    {
        var result = await mediator.Send(query);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Policy = Policies.AdminOrLeader)]
    public async Task<ActionResult<ProjectDto>> GetProjectById([FromRoute] Guid id)
    {
        var project = await mediator.Send(new GetProjectByIdQuery(id));
        if (project == null)
            return NotFound();
        return Ok(project);
    }

    [HttpPost("create")]
    [Authorize(Policy = Policies.LeaderOnly)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        var result = await mediator.Send(command);
        if(!result.IsSuccess)
            return BadRequest(result);
        return CreatedAtAction(nameof(GetProjectById), new { id=result.Data }, result);

    }

    [HttpPatch]
    [Route("{id}")]
    [Authorize(Policy = Policies.LeaderOnly)]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid id,UpdateProjectCommand command)
    {
        command.Id = id;
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

}
