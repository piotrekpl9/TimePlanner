using Domain.Group.Errors;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Primitives;
using Domain.Shared;
using Domain.User.ValueObjects;

namespace Domain.Group.Entities;

using User.Entities; 

public sealed class Invitation : Entity<InvitationId>
{
    internal Invitation(InvitationId id, UserId user, Member creator, InvitationStatus status, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        UserId = user;
        Creator = creator;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public UserId UserId { get; private set; }
    public Member Creator { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Result<Member> Accept()
    {
        switch (Status)
        {
            case InvitationStatus.Accepted:
                return Result<Member>.Failure(GroupError.InvitationAlreadyAccepted);
            
            case InvitationStatus.Expired:
                return Result<Member>.Failure(GroupError.InvitationAlreadyAccepted);   
        
            case InvitationStatus.Rejected:
                return Result<Member>.Failure(GroupError.InvitationAlreadyRejected);
        }

        Status = InvitationStatus.Accepted;
        UpdatedAt = DateTime.UtcNow;

        var member = Member.Create(this);
        
        return Result<Member>.Success(member);
    } 
    
    public Result Reject()
    {
        switch (Status)
        {
            case InvitationStatus.Accepted:
                return Result.Failure(GroupError.InvitationAlreadyAccepted);
            
            case InvitationStatus.Expired:
                return Result.Failure(GroupError.InvitationAlreadyExpired);   
        
            case InvitationStatus.Rejected:
                return Result.Failure(GroupError.InvitationAlreadyRejected);
        }

        Status = InvitationStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    } 
    
    public Result Expire()
    {
        switch (Status)
        {
            case InvitationStatus.Accepted:
                return Result.Failure(GroupError.InvitationAlreadyAccepted);
            
            case InvitationStatus.Expired:
                return Result.Failure(GroupError.InvitationAlreadyExpired);   
        
            case InvitationStatus.Rejected:
                return Result.Failure(GroupError.InvitationAlreadyRejected);
        }

        Status = InvitationStatus.Expired;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    } 
}