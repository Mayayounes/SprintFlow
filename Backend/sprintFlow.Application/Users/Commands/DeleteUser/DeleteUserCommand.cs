using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<string>>
{
    public string Email { get; set; } = default!;
}
