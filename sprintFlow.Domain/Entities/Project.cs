namespace sprintFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<Tasks> Tasks { get; set; } = new();
    //"managed by" (FK on user table)
}
