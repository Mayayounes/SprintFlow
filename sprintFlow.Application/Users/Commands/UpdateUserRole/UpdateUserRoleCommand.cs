using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommand : IRequest<Result<string>>
{
    public string Email { get; set; } = default!;
    public string NewRole { get; set; } = default!;
}
