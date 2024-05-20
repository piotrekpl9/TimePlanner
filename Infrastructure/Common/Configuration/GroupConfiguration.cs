using Domain.Group.Entities;
using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Configuration;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(group => group.Id);

        builder.Property(group => group.Id).HasConversion(
            groupId => groupId.Value,
            guid => new GroupId(guid));
        
        
        builder.HasMany<Member>(group => group.Members).WithOne().HasForeignKey(m => m.GroupId).IsRequired(true);
      
        builder.OwnsMany<Invitation>(m => m.Invitations,
            invitation =>
            {
                invitation.HasKey(i => i.Id);

                invitation.Property(i => i.Id).HasConversion(
                    invitationId => invitationId.Value,
                    guid => new InvitationId(guid));

                invitation.Property(i => i.UserId).HasConversion(
                    userId => userId.Value,
                    guid => new UserId(guid));
                
                invitation.Property(i => i.GroupId).HasConversion(
                    groupId => groupId.Value,
                    guid => new GroupId(guid));

            });
    }
}