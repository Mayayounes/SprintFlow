using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Roles;

public class GetRolesQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetRolesQuery, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllRolesAsync();

        return Result<List<string>>
            .Success(roles, "Roles retrieved successfully");
    }
}
