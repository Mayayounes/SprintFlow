using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;

namespace sprintFlow.Application.Users.Queries.GetNewUsers;

public class GetNewUsersQuery : IRequest<Result<List<UserDto>>>
{
    public string? Email { get; set; }
}
