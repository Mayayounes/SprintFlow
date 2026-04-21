using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<string>>
{
    public string UserId { get; set; } = default!;
    public string? UserName { get; set; } = null!;
    public string? Email { get; set; } = default!;
    public string? PhoneNumber { get; set; } = null!;
}
