using sprintFlow.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace sprintFlow.Domain.Entities;
public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; }
    [NotMapped]
    public DateTime CreatedAtLocal { get; set; }
    [NotMapped]
    public DateTime? UpdatedAtLocal { get; set; }
}