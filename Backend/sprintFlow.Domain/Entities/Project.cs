namespace sprintFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public string ManagerId { get; set; } = null!;
    public User Manager { get; set; } = null!;
}
