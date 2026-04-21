using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<string>.Failure(new List<string> { "User not found" });

        var role = await roleManager.FindByNameAsync(request.Role);
         if (role == null)
            return Result<string>.Failure(new List<string> { "Role not found" });

        var result = await userManager.AddToRoleAsync(user, role.Name!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<string>.Failure(errors, "Failed to assign role");
        }

        return Result<string>.Success("Role assigned successfully");
    }
}
