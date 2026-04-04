using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;

namespace sprintFlow.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(UserManager<User> userManager) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException(nameof(User), request.Email);

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception("Failed to delete user");
        }
    }
}
