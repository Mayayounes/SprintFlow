using FluentValidation;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommandValidator : AbstractValidator<UpdateTaskDetailsCommand>
{
    public UpdateTaskDetailsCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .Length(3, 100).WithMessage("Task title must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(3, 300).WithMessage("Description must be between 3 and 300 characters.");

        RuleFor(x => x.AssignedDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Assigned date cannot be in the past.");

        RuleFor(x => x.AssignedDate)
            .LessThan(x => x.Deadline)
            .WithMessage("Assigned date must be before deadline.");

    }
}
