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
            .Must(BeValidStatus)
            .WithMessage(GetAllowedValuesMessage);
    }
    private bool BeValidStatus(int? status)
    {
        if (!status.HasValue) return false;

        return Enum.IsDefined(typeof(TaskItemStatus), status.Value)
               && status.Value != (int)TaskItemStatus.ToDo;
    }

    private string GetAllowedValuesMessage(UpdateTaskStatusCommand command, int? status)
    {
        var allowedValues = string.Join(", ",
            Enum.GetValues(typeof(TaskItemStatus))
                .Cast<TaskItemStatus>()
                .Where(e => e != TaskItemStatus.ToDo)
                .Select(e => $"{e} = {(int)e}")
        );

        return $"Invalid status. Allowed values: [{allowedValues}]";
    }
}
