using FluentValidation;

namespace sprintFlow.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserName).MaximumLength(50);
        RuleFor(x => x.PhoneNumber).Matches(@"^\+?[0-9]{10,15}$");
    }
}
