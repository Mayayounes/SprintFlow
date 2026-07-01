using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Users.Commands.AddUser;

public class AddUserCommand : IRequest<Result<string>>
{
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole? Role { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string TimeZoneId { get; set; } = "UTC";

}
