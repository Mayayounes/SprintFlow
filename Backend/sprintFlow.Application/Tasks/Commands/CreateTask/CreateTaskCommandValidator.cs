using FluentValidation;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .Length(3, 200).WithMessage("Task title must be between 3 and 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(3, 1000).WithMessage("Description must be between 3 and 1000 characters.");

        RuleFor(x => x.AssignedDate)
            .NotEmpty().WithMessage("Assigned Date is required")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Assigned date cannot be in the past.");

        RuleFor(x => x.Deadline)
            .NotEmpty().WithMessage("Deadline is required")
            .GreaterThan(x => x.AssignedDate)
            .WithMessage("Assigned date must be before deadline.");
    }
}