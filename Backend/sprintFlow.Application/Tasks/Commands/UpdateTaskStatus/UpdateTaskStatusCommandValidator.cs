using FluentValidation;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("Enter status.")
            .Must(BeValidEnum)
            .WithMessage("Invalid status.");
    }
    private bool BeValidEnum(int? status)
    {
        return status.HasValue &&
               Enum.IsDefined(typeof(TaskItemStatus), status.Value);
    }

}
