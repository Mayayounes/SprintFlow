using MediatR;
using Microsoft.AspNetCore.Identity;
using sprintFlow.Application.Common;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(UserManager<User> userManager , IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        if (user == null)
            return Result<string>.Failure(new List<string> { "User not found" });

        var roles = await userManager.GetRolesAsync(user);

        if (await userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
        {
            return Result<string>.Failure(
                new List<string> { "Cannot delete an admin" }
            );
        }

        var userIdGuid = Guid.Parse(user.Id);
        if (roles.Contains(UserRole.Employee.ToString()))
        {
            var taskCount = await userRepository.CountEmployeeTasksAsync(userIdGuid);

            if (taskCount > 0)
            {
                return Result<string>.Failure(
                    new List<string> { $"Cannot delete employee : assigned to {taskCount} task(s)" }
                );
            }
        }
        if (roles.Contains(UserRole.Leader.ToString()))
        {
            var projectCount = await userRepository.CountLeaderProjectsAsync(userIdGuid);

            if (projectCount > 0)
            {
                return Result<string>.Failure(
                    new List<string> { $"Cannot delete leader : managing {projectCount} project(s)" }
                );
            }
        }
        var deleted = await userRepository.DeleteUserAsync(user);

        if (!deleted)
            return Result<string>.Failure(new List<string> { "Failed to delete user" });

        return Result<string>.Success("User deleted successfully");
    }
}
