using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedUsers<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;

    public GetAllUsersQueryHandler(IUserRepository userRepository, UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<PagedUsers<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        UserRoles? filterRole = null;

        // Check if role filter is provided
        if (!string.IsNullOrEmpty(request.role))
        {
            if (Enum.TryParse<UserRoles>(request.role, ignoreCase: true, out var parsedRole))
            {
                filterRole = parsedRole;
            }
            else
            {
                // Role does not exist in enum => return empty
                return new PagedUsers<UserDto>(new List<UserDto>(), 0, request.PageNumber, request.PageSize);
            }
        }

        // Get all users with pagination (repository does not filter by role)
        var (users, totalCount) = await _userRepository.GetAllMatchingAsync(
            role: null,
            pageSize: request.PageSize,
            pageNumber: request.PageNumber
        );

        var usersDto = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user); // actual roles
            var firstRoleName = roles.FirstOrDefault() ?? "";

            // Convert role name to enum if possible
            if (Enum.TryParse<UserRoles>(firstRoleName, ignoreCase: true, out var userRole))
            {
                // If filter exists, skip non-matching roles
                if (filterRole.HasValue && userRole != filterRole.Value)
                    continue;

                usersDto.Add(new UserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = userRole
                });
            }
            // else skip user if role is not in enum
        }

        // totalCount should reflect filtered users if role filter applied
        var filteredCount = filterRole.HasValue ? usersDto.Count : totalCount;

        return new PagedUsers<UserDto>(usersDto, filteredCount, request.PageNumber, request.PageSize);
    }
}
