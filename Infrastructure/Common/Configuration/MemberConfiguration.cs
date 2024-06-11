using Domain.Group.Entities;
using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Configuration;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(member => member.Id);
       
        builder.Property(member => member.Id).HasConversion(
            memberId => memberId.Value,
            guid => new MemberId(guid));
        
        builder.Property(member => member.GroupId).HasConversion(
            groupId => groupId.Value,
            guid => new GroupId(guid));
        
        builder.HasOne<Domain.User.Entities.User>(member => member.User).WithMany().HasForeignKey(m => m.UserId);
            
      
    }
}