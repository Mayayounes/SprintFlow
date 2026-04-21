using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Users.Commands.AssignUserRole;
using sprintFlow.Application.Users.Commands.DeleteUser;
using sprintFlow.Application.Users.Commands.Login;
using sprintFlow.Application.Users.Commands.UpdateUser;
using sprintFlow.Domain.Constants;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("/api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPost("assignRole")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
    [HttpPut("edit/{userId}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> UpdateUser([FromRoute] string userId,[FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;

        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpDelete("deleteUser")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> DeleteUser(DeleteUserCommand command)
    {
        var result = await mediator.Send(command);
        if(!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
}
