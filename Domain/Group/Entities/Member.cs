using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Group.Entities;
using Models.Enums;
using Primitives;
public sealed class Member : Entity<MemberId>
{
    private Member(MemberId id, UserId userId, GroupId groupId, Role role, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        UserId = userId;
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
    public UserId UserId { get; private set; }
    public Role Role { get; private set; }
    public GroupId GroupId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Member Create(GroupId groupId, Invitation invitation)
    {
        return new Member(new MemberId(Guid.NewGuid()), invitation.UserId, groupId, Role.Basic, DateTime.UtcNow, null, null);
    }
    
    public static Member CreateOwner(GroupId groupId, UserId userId)
    {
        return new Member(new MemberId(Guid.NewGuid()), userId, groupId, Role.Admin, DateTime.UtcNow, null, null);
    }
}