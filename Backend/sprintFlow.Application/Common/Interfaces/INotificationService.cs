using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Common.Interfaces;

public interface INotificationService
{
    Task<Notification> CreateAsync(Guid userId, string message);
    Task PublishAsync(Notification notification);
    Task SendAsync(Guid userId, string message);
    Task SendAsync(Notification notification);
    Task MarkAsReadAsync(Guid notificationId, Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
}