using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Tasks.Queries.GetMyTasks;
using sprintFlow.Domain.Constants;

namespace sprintFlow.API.Controllers;

    [ApiController]
    [Route("api/tasks")]
    public class MyTasksController(IMediator mediator) : ControllerBase
    {
    [HttpGet("my-tasks")]
    [Authorize(Roles = nameof(UserRole.Employee))]
    public async Task<IActionResult> GetMyTasks([FromQuery] int pageNumber ,[FromQuery] int pageSize, [FromQuery] string? status = null)
    {
        var result = await mediator.Send(new GetMyTasksQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Status = status
        });

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}