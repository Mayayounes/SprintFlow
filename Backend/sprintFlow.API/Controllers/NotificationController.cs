using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Notifications.Query.GetAllNotifications;

namespace sprintFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(IMediator mediator)
    : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] GetAllNotificationsQuery query)
    {
        var result = await mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}