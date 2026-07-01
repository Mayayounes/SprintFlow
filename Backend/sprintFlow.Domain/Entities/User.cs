using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace sprintFlow.Domain.Entities;

public class User : IdentityUser
{
    public List<Project> ManagedProjects { get; set; } = new List<Project>();
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public string TimeZoneId { get; set; } = "UTC";

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
}
