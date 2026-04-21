using FluentValidation;

namespace sprintFlow.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];
    public GetAllUsersQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in[{string.Join(",", allowPageSizes)}]");

    }
}
