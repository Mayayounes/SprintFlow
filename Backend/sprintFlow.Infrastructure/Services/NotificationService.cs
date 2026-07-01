using Microsoft.AspNetCore.SignalR;
using sprintFlow.Application.Common.Hubs;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Notifications.Dto;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Helpers;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Infrastructure.Services;

public class NotificationService(IHubContext<NotificationHub> hubContext , INotificationRepository notificationRepository , IUnitOfWork unitOfWork , IUserRepository userRepository) : INotificationService
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
        var unreadCount =await notificationRepository.GetUnreadCountAsync(notification.UserId);

        var user = await userRepository.GetByIdAsync(notification.UserId.ToString());

        var createdAtLocal =TimeZoneHelper.ToUserTime(notification.CreatedAt, user.TimeZoneId);


        await hubContext.Clients
            .User(notification.UserId.ToString())
            .SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Message,
                notification.CreatedAt,
                CreatedAtLocal = createdAtLocal,
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

        var notification = await notificationRepository.GetByIdAsync(notificationId);

        if (notification == null)
            return;

        var unreadCount = await notificationRepository.GetUnreadCountAsync(userId);

        var user = await userRepository.GetByIdAsync(userId.ToString());

        var dto = new NotificationDto
        {
            Id = notification.Id,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt,
            CreatedAtLocal = TimeZoneHelper.ToUserTime(notification.CreatedAt, user.TimeZoneId),
            UpdatedAtLocal = notification.UpdatedAt == null
                ? null
                : TimeZoneHelper.ToUserTime(notification.UpdatedAt.Value, user.TimeZoneId)
        };

        await hubContext.Clients.User(userId.ToString())
            .SendAsync("NotificationRead", new
            {
                notification = dto,
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