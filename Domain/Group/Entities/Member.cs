using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Group.Entities;
using Models.Enums;
using Primitives;
using User = User.Entities.User;
public class Member : Entity<MemberId>
{
    private Member(MemberId id, UserId userId, User user, GroupId groupId, Role role, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        User = user;
        Role = role;
        GroupId = groupId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
    private Member(MemberId id, Role role, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        Role = role;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
    public virtual User User { get; private set; }
    public UserId UserId { get; private set; }
    public Role Role { get; private set; }
    public GroupId GroupId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Member Create(GroupId groupId, User user)
    {
        return new Member(new MemberId(Guid.NewGuid()), user.Id, user, groupId, Role.Basic, DateTime.UtcNow, null, null);
    }
    
    public static Member CreateOwner(GroupId groupId, User user)
    {
        return new Member(new MemberId(Guid.NewGuid()), user.Id, user, groupId, Role.Admin, DateTime.UtcNow, null, null);
    }
}