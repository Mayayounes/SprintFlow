using System.ComponentModel.DataAnnotations;

namespace sprintFlow.Domain.Common;

public class BaseEntity
{
    public Guid Id { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
