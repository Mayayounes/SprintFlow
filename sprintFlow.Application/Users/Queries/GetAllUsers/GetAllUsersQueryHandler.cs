using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository) : IRequestHandler<GetAllUsersQuery, Result<PagedResults<UserDto>>>
{
    public async Task<Result<PagedResults<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await userRepository.GetAllMatchingAsync(request.Role, request.PageNumber, request.PageSize);

        if (!string.IsNullOrEmpty(request.Role) && !await roleRepository.RoleExistsAsync(request.Role))
        {
            return Result<PagedResults<UserDto>>.Failure(
                new List<string> { "Role does not exist" },
                "Validation failed"
            );
        }
        var usersDto = mapper.Map<IEnumerable<UserDto>>(users);

        var results = new PagedResults<UserDto>(usersDto, totalCount, request.PageNumber, request.PageSize);
        return Result<PagedResults<UserDto>>.Success(results, "Users retrieved successfully");
    }
}
