using Domain.Group.Errors;
using Domain.Group.Models.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Group.Entities;
using Domain.User.Entities;

public sealed class Group : AggregateRoot
{
    private readonly List<Task> _tasks;
    private readonly List<Invitation> _invitations;
    private readonly List<Member> _members;
    public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();
    public IReadOnlyCollection<Invitation> Invitations => _invitations.AsReadOnly();
    public IReadOnlyCollection<Member> Members => _members.AsReadOnly();
    public string Name { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Member Creator { get; set; }
    
    private Group(Guid id, Member creator,string name, List<Task> tasks, List<Invitation> invitations, List<Member> members, DateTime createdAt,DateTime? updatedAt,DateTime? deletedAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
        Creator = creator;
        _tasks = tasks;
        _invitations = invitations;
        _members = members;
    }

    public static Group Create(Guid id,string name, Guid userId)
    {
        var creator = Member.CreateOwner(userId);
        return new Group(new Guid(), creator, name, [], [], [creator], DateTime.UtcNow, null, null);
    }
    
    public Result<Invitation> SendInvitation(User user, Member sender)
    {
        if (Members.Any(e => e.UserId == user.Id))
        {
            return Result<Invitation>.Failure(GroupError.UserIsAMember);
        }
        
        if(Invitations.Any(e => e.UserId == user.Id))
        {
            return Result<Invitation>.Failure(GroupError.UserAlreadyInvited);
        }
        
        var invitation = new Invitation(new Guid(), user, sender, InvitationStatus.Pending,DateTime.UtcNow, null, null);
        
        _invitations.Add(invitation);

        return Result<Invitation>.Success(invitation);
    }
    
    public Result<Member> AcceptInvitation(Invitation invitation)
    {
        var memberResult = invitation.Accept();
        if (memberResult is { IsSuccess: true, Value: not null })
        {
            _members.Add(memberResult.Value);
            return Result<Member>.Success(memberResult.Value);
        }

        return Result<Member>.Failure(memberResult.Error);
    }
    
    public Result RejectInvitation(Invitation invitation)
    {
        var result = invitation.Reject();
        if (result.IsSuccess)
        {
            return Result.Success();
        }
        return Result.Failure(result.Error);
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