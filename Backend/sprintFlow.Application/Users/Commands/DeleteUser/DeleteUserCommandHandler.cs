using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(UserManager<User> userManager) : IRequestHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
            return Result<string>.Failure(new List<string> { "User Not Found." });

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return Result<string>.Failure(new List<string> { "Failed to delete user." });
        }
        return Result<string>.Failure(new List<string> { "User Deleted successfully." });

    }
}
