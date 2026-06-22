using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Notifications.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Notifications.Query.GetAllNotifications;


public class GetAllNotificationsQueryHandler(IUserContext userContext ,INotificationRepository notificationRepository ) : IRequestHandler<GetAllNotificationsQuery, Result<PagedResults<NotificationDto>>>
{
    public async Task<Result<PagedResults<NotificationDto>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var userId = Guid.Parse(currentUser!.Id);

        var (notifications, totalCount)
            = await notificationRepository.GetAllMatchingAsync(
                userId,
                request.Filter,
                request.PageNumber,
                request.PageSize);

        var dto = notifications.Select(x =>
            new NotificationDto
            {
                Id = x.Id,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();

        var pagedResult =
            new PagedResults<NotificationDto>(
                dto,
                totalCount,
                request.PageNumber,
                request.PageSize);

        return Result<PagedResults<NotificationDto>>
            .Success(
                pagedResult,
                "Notifications retrieved successfully");
    }
}
