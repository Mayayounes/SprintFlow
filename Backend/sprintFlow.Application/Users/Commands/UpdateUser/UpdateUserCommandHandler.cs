using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Users.Commands.UpdateUser;


public class UpdateUserCommandHandler(IUserRepository userRepository , IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return Result<UserDto>.Failure(
                new List<string> { "User not found" });
        }

        var submittedVersion = Convert.FromBase64String(request.RowVersion);

        await userRepository.SetOriginalRowVersion(user,submittedVersion);

        user.UserName = request.UserName ?? user.UserName;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.TimeZoneId = request.TimeZoneId ?? user.TimeZoneId;

        await unitOfWork.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = Guid.Parse(user.Id),
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
            RowVersion = Convert.ToBase64String(user.RowVersion),
            TimeZoneId = user.TimeZoneId,
        };

        return Result<UserDto>.Success(
            userDto,
            "User updated successfully");
    }
}
