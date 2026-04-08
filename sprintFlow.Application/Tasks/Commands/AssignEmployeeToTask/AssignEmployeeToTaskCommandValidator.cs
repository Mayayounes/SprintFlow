using FluentValidation;

namespace sprintFlow.Application.Tasks.Commands.AssignEmployeeToTask;

public class AssignEmployeeToTaskCommandValidator : AbstractValidator<AssignEmployeeToTaskCommand>
{
    public AssignEmployeeToTaskCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.");
    }
}
