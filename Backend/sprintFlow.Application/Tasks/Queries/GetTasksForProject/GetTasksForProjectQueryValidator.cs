using FluentValidation;

namespace sprintFlow.Application.Tasks.Queries.GetTasksForProject;

public class GetTasksForProjectQueryValidator : AbstractValidator<GetTaskForProjectQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];

    public GetTasksForProjectQueryValidator()
    {
        RuleFor(r => r.PageNumber)
    .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in[{string.Join(",", allowPageSizes)}]");
    }
}
