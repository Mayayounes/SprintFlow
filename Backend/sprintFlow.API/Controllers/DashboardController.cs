using MediatR;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.DashboardStats.Query;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await mediator.Send(new GetDashboardStatsQuery());
        return Ok(result);
    }
}
