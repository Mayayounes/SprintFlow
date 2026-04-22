using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Users.Commands.UpdateUser;


public class UpdateUserCommandHandler(UserManager<User> userManager) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        if (user == null)
            return Result<string>.Failure(new List<string> { "User not found" });

        user.UserName = request.UserName;
        user.PhoneNumber = request.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure(new List<string>{ errors });
        }

        return Result<string>.Success("User updated successfully");
    }
}
