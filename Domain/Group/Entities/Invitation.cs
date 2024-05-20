using Domain.Group.Errors;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Primitives;
using Domain.Shared;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Group.Entities;

using User.Entities; 

[Owned]
public sealed class Invitation : Entity<InvitationId>
{
    internal Invitation(InvitationId id, GroupId groupId, UserId user, Member creator, InvitationStatus status, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        UserId = user;
        Creator = creator;
        GroupId = groupId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
    private Invitation(InvitationId id, InvitationStatus status, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
    public UserId UserId { get; private set; }
    public Member Creator { get; private set; }
    public GroupId GroupId { get; private set; }
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
                return Result<Member>.Failure(GroupError.InvitationAlreadyExpired);   
        
            case InvitationStatus.Rejected:
                return Result<Member>.Failure(GroupError.InvitationAlreadyRejected);
            
            case InvitationStatus.Cancelled:
                return Result<Member>.Failure(GroupError.InvitationAlreadyCancelled);
        }

        Status = InvitationStatus.Accepted;
        UpdatedAt = DateTime.UtcNow;

        var member = Member.Create(GroupId,this);
        
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
            
            case InvitationStatus.Cancelled:
                return Result.Failure(GroupError.InvitationAlreadyCancelled);
        }

        Status = InvitationStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    } 
    
    public Result Cancel()
    {
        switch (Status)
        {
            case InvitationStatus.Accepted:
                return Result.Failure(GroupError.InvitationAlreadyAccepted);
            
            case InvitationStatus.Expired:
                return Result.Failure(GroupError.InvitationAlreadyExpired);   
        
            case InvitationStatus.Rejected:
                return Result.Failure(GroupError.InvitationAlreadyRejected);
            
            case InvitationStatus.Cancelled:
                return Result.Failure(GroupError.InvitationAlreadyCancelled);
        }

        Status = InvitationStatus.Cancelled;
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
            
            case InvitationStatus.Cancelled:
                return Result.Failure(GroupError.InvitationAlreadyCancelled);
        }

        Status = InvitationStatus.Expired;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    } 
}