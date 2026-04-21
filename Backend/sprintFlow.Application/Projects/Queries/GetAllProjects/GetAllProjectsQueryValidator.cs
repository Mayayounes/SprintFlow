using FluentValidation;

namespace sprintFlow.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryValidator: AbstractValidator<GetAllProjectsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];
    public GetAllProjectsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in[{string.Join(",", allowPageSizes)}]");

    }
}
