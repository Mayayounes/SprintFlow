using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Users.Commands.AddUser;
using sprintFlow.Application.Users.Commands.Login;
using sprintFlow.Domain.Constants;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("/api")]

public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpPost("addUser")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> AddUser(AddUserCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
