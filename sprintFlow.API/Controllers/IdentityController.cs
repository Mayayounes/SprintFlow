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
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("changeRole")]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public async Task<IActionResult> UpdateUserRole(UpdateUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("deleteUser")]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public async Task<IActionResult> DeleteUser(DeleteUserCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
