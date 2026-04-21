using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<string>>
{
    public string UserId { get; set; } = default!;
}
