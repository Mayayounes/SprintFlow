using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Domain.Common;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    }
    //public override async Task<int> SaveChangesAsync(
    //CancellationToken cancellationToken = default)
    //{
    //    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    //    {
    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.CreatedAt = DateTime.UtcNow;
    //        }

    //        if (entry.State == EntityState.Modified)
    //        {
    //            entry.Entity.UpdatedAt = DateTime.UtcNow;
    //        }
    //    }

    //    return await base.SaveChangesAsync(cancellationToken);
    //}
}