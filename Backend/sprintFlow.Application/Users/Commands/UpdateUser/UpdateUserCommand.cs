using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;


namespace sprintFlow.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<UserDto>>
{
    public string UserId { get; set; } = default!;
    public string? UserName { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string RowVersion { get; set; } = default!;
    public string TimeZoneId { get; set; } = "UTC";

}
