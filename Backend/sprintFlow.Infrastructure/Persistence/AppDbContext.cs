using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }
    internal DbSet<Project> Projects { get; set; }
    internal DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Manager)
            .WithMany(u => u.ManagedProjects) 
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Employee)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
