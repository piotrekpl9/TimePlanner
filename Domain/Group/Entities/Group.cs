using Domain.Group.Errors;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Primitives;
using Domain.Shared;
using Domain.User.Errors;
using Domain.User.ValueObjects;

namespace Domain.Group.Entities;
using Domain.User.Entities;

public sealed class Group : AggregateRoot<GroupId>
{
    private readonly List<Invitation> _invitations = new List<Invitation>();
    private readonly List<Member> _members = new List<Member>();
    public IReadOnlyCollection<Invitation> Invitations => _invitations.ToList();
    public IReadOnlyCollection<Member> Members => _members.ToList();
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    
    private Group(GroupId id, string name, List<Invitation> invitations, List<Member> members, DateTime createdAt,DateTime? updatedAt,DateTime? deletedAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;        
        _members = members;
        _invitations = invitations;
    }

    private Group(GroupId id,string name, DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
    
    public static Group Create(string name, User user)
    {
        var groupGuid = new GroupId(Guid.NewGuid());
        var creator = Member.CreateOwner(groupGuid, user);
        return new Group(groupGuid,  name, [], [creator], DateTime.UtcNow, null, null);
    }
    
    public Result<Invitation> Invite(User user, UserId senderId)
    {
        if(Invitations.Any(e => e.User.Id.Equals(user.Id) && e.Status is InvitationStatus.Pending))
        {
            return Result<Invitation>.Failure(GroupError.UserAlreadyInvited);
        }
        
        if (Members.Any(member => member.User.Id.Equals(user.Id) && member.DeletedAt is null))
        {
            return Result<Invitation>.Failure(GroupError.UserIsAMember);
        }
        
        if (DeletedAt is not null)
        {
            return Result<Invitation>.Failure(GroupError.GroupIsDeleted);
        }

        var sender = Members.FirstOrDefault(member => member.User.Id.Equals(senderId));
        if (sender is null)
        {
            return Result<Invitation>.Failure(GroupError.UserIsNotMember);
        }
        
        var invitation = new Invitation( new InvitationId(Guid.NewGuid()), Id, user, sender, InvitationStatus.Pending,DateTime.UtcNow, null, null);
        
        _invitations.Add(invitation);

        return Result<Invitation>.Success(invitation);
    }
    
    public Result<Member> AcceptInvitation(Invitation invitation)
    {
        if (!invitation.User.Id.Equals(invitation.User.Id))
        {
            return Result<Member>.Failure(UserError.IdDoesntMatch);
        }
        var acceptResult = invitation.Accept();
       
        if (acceptResult.IsSuccess)
        {
            var member = Member.Create(invitation.GroupId, invitation.User);
            _members.Add(member);
            return Result<Member>.Success(member);
        }

        return Result<Member>.Failure(acceptResult.Error);
    }
    
    public Result RejectInvitation(Invitation invitation)
    {
        return invitation.Reject();
    }
    
    public Result CancelInvitation(Invitation invitation)
    {
        return invitation.Cancel();
    }
    public Result RemoveMember(MemberId targetMemberId)
    {
        var member = _members.FirstOrDefault(member => member.Id.Equals(targetMemberId));
        if (member is null)
        {
            return Result.Failure(GroupError.MemberNotFound);
        }
        
        var result = _members.Remove(member);
        
        
        if (member.Role == Role.Admin)
        {
            if (Members.All(m => m.Role != Role.Admin))
            {
                var firstMember = Members.FirstOrDefault();
                firstMember?.SetRole(Role.Admin);
            }
        }
        
        return result ? Result.Success() : Result.Failure(GroupError.FailedToRemoveMember);
    }
    public Result Delete()
    {
        if (DeletedAt is not null)
        {
            return Result.Failure(GroupError.GroupAlreadyDeleted);
        }
        DeletedAt = DateTime.UtcNow;
        return Result.Success();
    }
}