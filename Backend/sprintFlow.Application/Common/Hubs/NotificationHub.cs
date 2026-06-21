using Microsoft.AspNetCore.SignalR;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Domain.Repositories;
using System.Security.Claims;

namespace sprintFlow.Application.Common.Hubs;

public class NotificationHub : Hub
{
    private readonly INotificationRepository _repository;
    private readonly INotificationService _notificationService;

    public NotificationHub(INotificationRepository repository , INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task MarkAsRead(Guid notificationId)
    {
        var userId = Guid.Parse(
            Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _notificationService.MarkAsReadAsync(notificationId, userId);
    }

    public async Task MarkAllAsRead()
    {
        var userId = Guid.Parse(
            Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _notificationService.MarkAllAsReadAsync(userId);
    }

    public async Task<int> GetUnreadCount()
    {
        var userId = Guid.Parse(
            Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        return await _repository.GetUnreadCountAsync(userId);
    }
}