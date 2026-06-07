using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Commands.UpdateUser;


public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, Result<UserConcurrencyDto>>
{
    public async Task<Result<UserConcurrencyDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return Result<UserConcurrencyDto>.Failure(
                new List<string> { "User not found" });
        }

        user.UserName = request.UserName ?? user.UserName;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;

        var submittedVersion =
            Convert.FromBase64String(request.RowVersion);

        var result =
            await userRepository.UpdateUserAsync(
                user,
                submittedVersion);

        if (!result.success)
        {
            return Result<UserConcurrencyDto>.Failure(
                new List<string>
                {
                    "This user was modified by another process."
                },
                "ConcurrencyConflict",
                new UserConcurrencyDto
                {
                    UserId = result.user!.Id,
                    UserName = result.user.UserName,
                    Email = result.user.Email,
                    PhoneNumber = result.user.PhoneNumber,
                    RowVersion =
                        Convert.ToBase64String(
                            result.user.RowVersion)
                });
        }

        return Result<UserConcurrencyDto>.Success(
            new UserConcurrencyDto
            {
                UserId = result.user!.Id,
                UserName = result.user.UserName,
                Email = result.user.Email,
                PhoneNumber = result.user.PhoneNumber,
                RowVersion =
                    Convert.ToBase64String(
                        result.user.RowVersion)
            },
            "User updated successfully");
    }
}
