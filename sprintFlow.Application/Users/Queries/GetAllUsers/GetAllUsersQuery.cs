using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;

namespace sprintFlow.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<PagedResults<UserDto>>
{
    public string? role { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

}
