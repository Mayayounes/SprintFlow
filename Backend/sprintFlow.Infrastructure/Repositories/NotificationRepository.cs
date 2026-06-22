using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;
using sprintFlow.Infrastructure.Persistence;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sprintFlow.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _context.Notifications
            .CountAsync(x => x.UserId == userId && !x.IsRead);
    }

    public async Task<Notification?> GetByIdAsync(Guid notificationId)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == notificationId);
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

        if (notification == null) return;

        notification.IsRead = true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.UpdatedAt, DateTime.UtcNow)
            );
        // generated SQL:
        // UPDATE Notifications
        // SET IsRead = 1, UpdatedAt = GETUTCDATE()
        // WHERE UserId = @userId AND IsRead = 0
    }
    public async Task<(List<Notification>, int)> GetAllMatchingAsync(
    Guid userId,
    NotificationFilter filter,
    int pageNumber,
    int pageSize)
    {
        var query = _context.Notifications
            .Where(x => x.UserId == userId);

        query = filter switch
        {
            NotificationFilter.Unread =>
                query.Where(x => !x.IsRead),

            NotificationFilter.Seen =>
                query.Where(x => x.IsRead),

            _ => query
        };

        var totalCount = await query.CountAsync();

        var notifications = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (notifications, totalCount);
    }
}