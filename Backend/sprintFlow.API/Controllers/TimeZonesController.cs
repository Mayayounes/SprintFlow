using MediatR;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Users.Queries.GetTimeZones;

namespace sprintFlow.API.Controllers;

[ApiController]
    public class TimeZonesController(IMediator mediator) : ControllerBase
    {
        [HttpGet("time-zones")]
        public async Task<IActionResult> GetTimeZones()
        {
            var result = await mediator.Send(new GetTimeZonesQuery());

            return Ok(result);
        }
    }
