using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<UserConcurrencyDto>>
{
    public string UserId { get; set; } = default!;
    public string? UserName { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string RowVersion { get; set; } = default!;

}
