using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Notifications.Dto;
using sprintFlow.Application.Users;

namespace sprintFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IUserContext _userContext;

    public NotificationsController(
        INotificationService notificationService,
        IUserContext userContext)
    {
        _notificationService = notificationService;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        var user = _userContext.GetCurrentUser();
        var userId = Guid.Parse(user!.Id);

        var notifications = await _notificationService.GetUserNotificationsAsync(userId);

        var result = notifications.Select(x => new NotificationDto
        {
            Id = x.Id,
            Message = x.Message,
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt
        }).ToList();

        return Ok(result);
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        var user = _userContext.GetCurrentUser();
        var userId = Guid.Parse(user!.Id);

        var count = await _notificationService.GetUnreadCountAsync(userId);

        return Ok(count);
    }

    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId)
    {
        var user = _userContext.GetCurrentUser();
        var userId = Guid.Parse(user!.Id);

        await _notificationService.MarkAsReadAsync(notificationId, userId);

        return NoContent();
    }
    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var user = _userContext.GetCurrentUser();

        var userId = Guid.Parse(user!.Id);

        await _notificationService.MarkAllAsReadAsync(userId);

        return NoContent();
    }
}