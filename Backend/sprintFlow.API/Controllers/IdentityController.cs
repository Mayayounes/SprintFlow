using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Users.Commands.DeleteUser;
using sprintFlow.Application.Users.Commands.Login;
using sprintFlow.Application.Users.Commands.UpdateUser;
using sprintFlow.Domain.Constants;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("/api/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPut("edit/{userId}")]
    [Authorize(Policy = Policies.AdminOnly)]
    public async Task<IActionResult> UpdateUser([FromRoute] string userId,[FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;

        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpDelete("deleteUser/{userId}")]
    [Authorize(Policy = Policies.AdminOnly)]    public async Task<IActionResult> DeleteUser([FromRoute] string userId)
    {
        var command = new DeleteUserCommand { UserId = userId };

        var result = await mediator.Send(command);
        if(!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
}
