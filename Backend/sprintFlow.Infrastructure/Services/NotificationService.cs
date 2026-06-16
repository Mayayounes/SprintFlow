using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.API.Hubs;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(
        INotificationRepository repository,
        IHubContext<NotificationHub> hubContext)
    {
        _repository = repository;
        _hubContext = hubContext;
    }

    public async Task SendAsync(Guid userId, string message)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await SendAsync(notification);
    }

    public async Task SendAsync(Notification notification)
    {
        await _repository.AddAsync(notification);
        await _repository.SaveChangesAsync();

        var unreadCount = await _repository.GetUnreadCountAsync(notification.UserId);

        await _hubContext.Clients
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

    public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
    {
        return await _repository.GetUserNotificationsAsync(userId);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _repository.GetUnreadCountAsync(userId);
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        await _repository.MarkAsReadAsync(notificationId, userId);
        await _repository.SaveChangesAsync();
    }
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await _repository.MarkAllAsReadAsync(userId);

        await _repository.SaveChangesAsync();
    }
}