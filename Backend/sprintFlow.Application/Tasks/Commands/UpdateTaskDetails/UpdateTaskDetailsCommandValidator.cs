using FluentValidation;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommandValidator : AbstractValidator<UpdateTaskDetailsCommand>
{
    public UpdateTaskDetailsCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .Length(3, 200).WithMessage("Task title must be between 3 and 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(3, 1000).WithMessage("Description must be between 3 and 1000 characters.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Deadline must be in the future.");
    }
}
