using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Users.Commands.AddUser;

public class AddUserCommandHandler(UserManager<User> userManager) : IRequestHandler<AddUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.UserName) ||
            request.Role == null)
        {

            return Result<string>.Failure(new List<string> { "Invalid user data" });
        }
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result<string>.Failure(new List<string> { "User already exists with this email" });
        }
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user, request.Password);
        await userManager.AddToRoleAsync(user, request.Role.ToString());
        return Result<string>.Success(user.Id);
    }
}
