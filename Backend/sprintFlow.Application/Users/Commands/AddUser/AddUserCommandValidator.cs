using FluentValidation;

namespace sprintFlow.Application.Users.Commands.AddUser;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[0-9]{10,15}$")
            .WithMessage("Phone number must be 10–15 digits and can start with +");
    }
}
