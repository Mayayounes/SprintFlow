using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest<Result<string>>
{
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}
