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

public class GetAllUsersQueryHandler(IUserRepository userRepository ,IMapper mapper , IRoleRepository roleRepository) : IRequestHandler<GetAllUsersQuery, PagedResults<UserDto>>
{
    public async Task<PagedResults<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await userRepository.GetAllMatchingAsync(request.role, request.PageNumber, request.PageSize);

        if (!string.IsNullOrEmpty(request.role) && !await roleRepository.RoleExistsAsync(request.role))
        {
            throw new Exception("Role does not exist");
        }
        var usersDto = mapper.Map<IEnumerable<UserDto>>(users);

        var results = new PagedResults<UserDto>(usersDto, totalCount, request.PageNumber, request.PageSize);
        return results;
    }
}
