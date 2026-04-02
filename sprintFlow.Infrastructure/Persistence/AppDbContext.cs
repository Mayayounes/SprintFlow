using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Infrastructure.Persistence;

internal class AppDbContext : DbContext
{
    internal DbSet<Project> Projects { get; set; }

    internal DbSet<Tasks> Tasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne()
            .HasForeignKey(t => t.ProjectID);
    }

}
