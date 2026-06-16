using sprintFlow.Domain.Common;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; }
}