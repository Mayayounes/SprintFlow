using FluentValidation;

namespace sprintFlow.Application.Notifications.Query.GetAllNotifications;

public class GetAllNotificationQueryValidator : AbstractValidator<GetAllNotificationsQuery>
{
    public int[] allowedPageSizes = [5, 10, 15, 40];
    public GetAllNotificationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .Must(size => allowedPageSizes.Contains(size))
            .WithMessage($"Page Size must be one of [{string.Join(",", allowedPageSizes)}]");
    }
}
