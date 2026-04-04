using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Application.Users.Queries.GetAllUsers;
using sprintFlow.Domain.Constants;

namespace sprintFlow.api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public async Task<ActionResult<PagedResults<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query)
    {
        var users = await mediator.Send(query);
        return Ok(users);
    }

}
