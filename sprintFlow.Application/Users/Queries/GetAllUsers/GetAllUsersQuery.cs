using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;

namespace sprintFlow.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<Result<PagedResults<UserDto>>>
{
    public string? Role { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

}
