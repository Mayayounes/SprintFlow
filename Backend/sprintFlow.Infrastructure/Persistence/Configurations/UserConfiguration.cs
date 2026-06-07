using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.ManagedProjects)
            .WithOne(p => p.Manager)
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Tasks)
            .WithOne(t => t.Employee)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
