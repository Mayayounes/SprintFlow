using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Notifications.Dto;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Notifications.Query.GetAllNotifications;

public class GetAllNotificationsQuery : IRequest<Result<PagedResults<NotificationDto>>>
{
    public NotificationFilter Filter { get; set; }= NotificationFilter.All;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
