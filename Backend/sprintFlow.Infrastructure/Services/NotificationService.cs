using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common.Hubs;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Services;

public class NotificationService(IHubContext<NotificationHub> hubContext , INotificationRepository notificationRepository , IUnitOfWork unitOfWork) : INotificationService
{
    public async Task<Notification> CreateAsync(Guid userId, string message)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await notificationRepository.AddAsync(notification);

        return notification;
    }
    public async Task PublishAsync(Notification notification)
    {
        var unreadCount =
            await notificationRepository.GetUnreadCountAsync(notification.UserId);

        await hubContext.Clients
            .User(notification.UserId.ToString())
            .SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Message,
                notification.CreatedAt,
                notification.IsRead,
                unreadCount
            });
    }
    public async Task SendAsync(Notification notification)
    {
        await notificationRepository.AddAsync(notification);
    }
    public async Task SendAsync(Guid userId, string message)
    { 
        var notification = new Notification { 
            Id = Guid.NewGuid(), 
            UserId = userId, 
            Message = message, 
            CreatedAt = DateTime.UtcNow, 
            IsRead = false 
        }; 
        await SendAsync(notification); }
    public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        await notificationRepository.MarkAsReadAsync(notificationId, userId);
        await unitOfWork.SaveChangesAsync();
        var unreadCount = await notificationRepository.GetUnreadCountAsync(userId);
        await hubContext.Clients.User(userId.ToString())
            .SendAsync("NotificationRead", new
            {
                notificationId,
                unreadCount
            });
    }
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await notificationRepository.MarkAllAsReadAsync(userId);
        await unitOfWork.SaveChangesAsync();
        await hubContext.Clients
            .User(userId.ToString())
            .SendAsync("UnreadCountUpdated", 0);

        await hubContext.Clients
            .User(userId.ToString())
            .SendAsync("AllNotificationsRead");
    }
}