using MediatR;

namespace sprintFlow.Application.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommand : IRequest
{
    public string Email { get; set; } = default!;
    public string NewRole { get; set; } = default!;
}
