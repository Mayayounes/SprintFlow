using MediatR;

namespace sprintFlow.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string Email { get; set; } = default!;
}
