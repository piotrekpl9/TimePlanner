namespace Domain.Group.Entities;
using Models.Enums;
using Primitives;
public sealed class Member : Entity
{
    private Member(Guid id, Guid userId, Role role, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        UserId = userId;
        Role = role;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public Guid UserId { get; private set; }
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Member Create(Invitation invitation)
    {
        return new Member(new Guid(), invitation.UserId, Role.Basic, DateTime.UtcNow, null, null);
    }
    
    public static Member CreateOwner(Guid userId)
    {
        return new Member(new Guid(), userId, Role.Admin, DateTime.UtcNow, null, null);
    }
}