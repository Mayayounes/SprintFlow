using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;

namespace sprintFlow.Application.Users.Commands.Login;

public class LoginCommand() : IRequest<Result<LoginDto>>
{
    public string email { get; set; } = default!;
    public string password { get; set; } = default!;
}
