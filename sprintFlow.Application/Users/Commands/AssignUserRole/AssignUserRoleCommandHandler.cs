using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;

namespace sprintFlow.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException(nameof(User), request.Email);
        var role = await roleManager.FindByNameAsync(request.Role)
            ?? throw new NotFoundException(nameof(IdentityRole), request.Role);
        await userManager.AddToRoleAsync(user, role.Name!);
    }
}
