using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Infrastructure.Persistence.Configurations;

internal class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Status)
            .HasConversion<int>()
            .HasDefaultValue(TaskItemStatus.ToDo)
            .IsRequired();

        builder.Property(t => t.AssignedDate)
             .IsRequired()
            .HasColumnType("date");

        builder.Property(t => t.Deadline)
             .IsRequired()
            .HasColumnType("date");

        builder.HasOne(t => t.Employee)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.StartedAt)
            .IsRequired(false);

        builder.Property(t => t.CompletedAt)
            .IsRequired(false);

        builder.Property(x => x.RowVersion)
       .IsRowVersion();
    }
}
