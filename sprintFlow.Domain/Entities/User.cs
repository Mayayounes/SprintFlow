using Microsoft.AspNetCore.Identity;

namespace sprintFlow.Domain.Entities;

public class User : IdentityUser
{
    public List<Project> ManagedProjects { get; set; } = new List<Project>();
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
