using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Users.Commands.UpdateUserRole;


public class UpdateUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<UpdateUserRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<string>.Failure(new List<string> { "User not found" });
        var roleExists = await roleManager.RoleExistsAsync(request.NewRole);
        if (!roleExists)
            return Result<string>.Failure(new List<string> { "Role not found" });
        var currentRole = await userManager.GetRolesAsync(user);
        if (currentRole.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRole);
            if (!removeResult.Succeeded)
            {
                var errors = removeResult.Errors.Select(e => e.Description).ToList();
                return Result<string>.Failure(errors, "Failed to remove old roles");
            }
        }
        var addResult = await userManager.AddToRoleAsync(user, request.NewRole);
        if (!addResult.Succeeded)
        {
            var errors = addResult.Errors.Select(e => e.Description).ToList();
            return Result<string>.Failure(errors, "Failed to assign new role");
        }
        return Result<string>.Success("Role updated successfully");

    }
}
