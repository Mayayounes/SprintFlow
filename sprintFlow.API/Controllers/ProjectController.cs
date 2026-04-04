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
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects([FromQuery] GetAllProjectsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById([FromRoute] Guid id)
    {
        var project = await mediator.Send(new GetProjectByIdQuery(id));
        if (project == null)
            return NotFound();
        return Ok(project);
    }

    [HttpPost("create")]
    [Authorize(Roles =nameof(UserRoles.Leader))]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        Guid id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetProjectById), new { id }, null);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid id,UpdateProjectCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

}
