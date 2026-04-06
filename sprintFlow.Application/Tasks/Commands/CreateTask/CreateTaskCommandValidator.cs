using FluentValidation;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.AssignedDate)
            .LessThan(x => x.Deadline)
            .WithMessage("Assigned date must be before deadline.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("EmployeeId is required.");
    }
}