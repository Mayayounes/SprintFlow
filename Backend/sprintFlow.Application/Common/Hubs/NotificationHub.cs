using Microsoft.AspNetCore.SignalR;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Domain.Repositories;
using System.Security.Claims;

namespace sprintFlow.Application.Common.Hubs;

public class NotificationHub(INotificationRepository notificationRepository , INotificationService notificationService) : Hub
{
    public async Task MarkAsRead(Guid notificationId)
    {
        var userId = Guid.Parse(
            Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await notificationService.MarkAsReadAsync(notificationId, userId);
    }

    public async Task MarkAllAsRead()
    {
        var userId = Guid.Parse(
            Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await notificationService.MarkAllAsReadAsync(userId);
    }

    public async Task<int> GetUnreadCount()
    {
        var userId = Guid.Parse(
            Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        return await notificationRepository.GetUnreadCountAsync(userId);
    }
}