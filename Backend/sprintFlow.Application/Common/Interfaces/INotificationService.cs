using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendAsync(Guid userId, string message);

    Task SendAsync(Notification notification);

    Task<List<Notification>> GetUserNotificationsAsync(Guid userId);

    Task<int> GetUnreadCountAsync(Guid userId);

    Task MarkAsReadAsync(Guid notificationId, Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
}