using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Users.Commands.AssignUserRole;
using sprintFlow.Application.Users.Commands.DeleteUser;
using sprintFlow.Application.Users.Commands.UpdateUserRole;
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

    [HttpPatch("changeRole")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> UpdateUserRole(UpdateUserRoleCommand command)
    {
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
