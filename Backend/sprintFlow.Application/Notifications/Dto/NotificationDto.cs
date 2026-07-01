namespace sprintFlow.Application.Notifications.Dto;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Local times
    public DateTime CreatedAtLocal { get; set; }
    public DateTime? UpdatedAtLocal { get; set; }
}