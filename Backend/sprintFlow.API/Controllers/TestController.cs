using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Common.Exceptions;

namespace sprintFlow.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        throw new ConcurrencyException();
    }
}