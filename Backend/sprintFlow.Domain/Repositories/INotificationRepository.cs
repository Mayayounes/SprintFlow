using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Domain.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task<Notification?> GetByIdAsync(Guid notificationId);
    Task MarkAsReadAsync(Guid notificationId, Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    Task<(List<Notification>, int)> GetAllMatchingAsync(Guid userId,NotificationFilter filter,int pageNumber,int pageSize);
}