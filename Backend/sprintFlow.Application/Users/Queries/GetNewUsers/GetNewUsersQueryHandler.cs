using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Queries.GetNewUsers;

public class GetNewUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetNewUsersQuery, Result<List<UserDto>>>
{
    public async Task<Result<List<UserDto>>> Handle(GetNewUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersWithoutRolesAsync();

        var usersDto = users.Select(user => new UserDto
        {
            UserName = user.UserName!,
            Email = user.Email!,
            Role = null,
            PhoneNumber = user.PhoneNumber!,
            Id = Guid.Parse(user.Id),
        }).ToList();

        return Result<List<UserDto>>
            .Success(usersDto, "New users retrieved successfully");
    }

}