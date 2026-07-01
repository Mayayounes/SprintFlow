using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository) : IRequestHandler<GetAllUsersQuery, Result<PagedResults<UserDto>>>
{
    public async Task<Result<PagedResults<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.searchRole) && !await roleRepository.RoleExistsAsync(request.searchRole))
        {
            return Result<PagedResults<UserDto>>.Failure(
                new List<string> { "Role does not exist" },
                "Validation failed"
            );
        }
        var (users, totalCount) = await userRepository.GetAllMatchingAsync(request.searchRole, request.PageNumber, request.PageSize);
        var userIds = users.Select(u => u.Id).ToList();

        var userRoles = await roleRepository.GetRolesForUsersAsync(userIds);
        var usersDto = new List<UserDto>();
        usersDto = users.Select(user =>
        {
            var roleName = userRoles.TryGetValue(user.Id, out var r) ? r : null;

            var parsedRole = Enum.TryParse<UserRole>(roleName, true, out var roleEnum)
                ? roleEnum
                : default;

            return new UserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Role = parsedRole,
                PhoneNumber = user.PhoneNumber!,
                RowVersion = Convert.ToBase64String(user.RowVersion),
                Id = Guid.Parse(user.Id),
                TimeZoneId = user.TimeZoneId,
            };
        }).ToList();
        var results = new PagedResults<UserDto>(usersDto,totalCount,request.PageNumber,request.PageSize);
        return Result<PagedResults<UserDto>>.Success(results, "Users retrieved successfully");
    }
}
