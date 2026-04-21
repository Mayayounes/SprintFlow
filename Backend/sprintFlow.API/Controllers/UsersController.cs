using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Common;
using sprintFlow.Application.Roles;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Application.Users.Queries.GetAllUsers;
using sprintFlow.Domain.Constants;

namespace sprintFlow.api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Policies.AdminOnly)]
    public async Task<ActionResult<PagedResults<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query)
    {
        var users = await mediator.Send(query);
        return Ok(users);
    }
    [HttpGet("roles")]
    [Authorize(Policy = Policies.AdminOnly)]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        var roles = await mediator.Send(new GetRolesQuery());
        if (!roles.IsSuccess)
            return BadRequest(roles);

        return Ok(roles.Data);
    }
}
