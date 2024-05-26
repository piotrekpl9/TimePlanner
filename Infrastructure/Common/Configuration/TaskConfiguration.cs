using System.Text.RegularExpressions;
using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.ValueObjects;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = Domain.Task.Entities.Task;
using Group = Domain.Group.Entities.Group;

namespace Infrastructure.Common.Configuration;
public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(task => task.Id);

        builder.Property(task => task.Id).HasConversion(
            taskId => taskId.Value,
            guid => new TaskId(guid));
        
        builder.HasOne<Group>().WithMany().HasForeignKey(task => task.GroupId).IsRequired(false);

       
        builder.Property(task => task.GroupId).HasConversion(
            groupId => groupId != null ? groupId.Value : default,
            guid => new GroupId(guid));
        
        builder.HasMany<Domain.User.Entities.User>(task => task.AssignedUsers).WithMany();
        builder.HasOne<Domain.User.Entities.User>(task => task.Creator).WithMany();
    }
}