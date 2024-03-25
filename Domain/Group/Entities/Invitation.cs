using Domain.Group.Models.Enums;
using Domain.Primitives;

namespace Domain.Group.Entities;

using User.Entities; 

public class Invitation : Entity
{
    internal Invitation(Guid id, User user, Member creator, InvitationStatus status, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        UserId = user.Id;
        Creator = creator;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public Guid UserId { get; private set; }
    public Member Creator { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
}