using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;

namespace sprintFlow.Application.Users.Commands.UpdateUserRole;


public class UpdateUserRoleCommandHandler(UserManager<User> userManager , RoleManager<IdentityRole> roleManager) : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException(nameof(User), request.Email);
        var roleExists = await roleManager.RoleExistsAsync(request.NewRole);
        if (!roleExists)
            throw new NotFoundException(nameof(IdentityRole), request.NewRole);
        var currentRole = await userManager.GetRolesAsync(user);
        if (currentRole.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRole);
            if (!removeResult.Succeeded)
                throw new Exception("Failed to remove old role");
        }
        var addResult = await userManager.AddToRoleAsync(user, request.NewRole);
        if (!addResult.Succeeded)
            throw new Exception("Failed to assign new role");
    }
}
