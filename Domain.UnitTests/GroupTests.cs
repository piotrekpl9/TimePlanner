using Domain.Group.Entities;
using Domain.Group.Errors;
using Domain.Group.Models.Enums;
using Domain.Shared;
using Domain.User.Entities;
using Domain.User.ValueObjects;

namespace Test;

public class GroupTests
{
    [Fact]
    public void Create_Should_AssignCreatingUserAdminRole()
    {
        var userResult = User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var user = userResult.Value;
        Assert.NotNull(user);
        
        var result = Group.Create("Kowalscy", user.Id);
        
        var member = result.Members.FirstOrDefault(member => member.UserId.Equals(user.Id));
        Assert.NotNull(member);
        Assert.Equal(Role.Admin,member.Role);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void Invite_Should_ReturnSuccessAndAddToInvitationsList(User target, UserId sender, Group group)
    {
        var result = group.Invite(target, sender);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Contains(result.Value, group.Invitations);
    }
      
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void Invite_Should_ReturnFailure_WhenTargetAlreadyInvited(User target, UserId sender, Group group)
    {
        var firstInvitation = group.Invite(target, sender);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);

        var secondInvitation = group.Invite(target, sender);

        Assert.True(secondInvitation.IsFailure);
        Assert.NotStrictEqual(Result<Invitation>.Failure(GroupError.UserAlreadyInvited),secondInvitation);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void Invite_Should_ReturnFailure_WhenTargetAlreadyInGroup(User target, UserId sender, Group group)
    {
        var firstInvitation = group.Invite(target, sender);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);
        group.AcceptInvitation(firstInvitation.Value);

        var secondInvitation = group.Invite(target, sender);

        Assert.True(secondInvitation.IsFailure);
        Assert.NotStrictEqual(Result<Invitation>.Failure(GroupError.UserIsAMember),secondInvitation);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void Invite_Should_ReturnFailure_WhenGroupDeleted(User target, UserId sender, Group group)
    {
        var res = group.Delete();
        Assert.True(res.IsSuccess);

        var invitation = group.Invite(target, sender);

        Assert.True(invitation.IsFailure);
        Assert.NotStrictEqual(Result<Invitation>.Failure(GroupError.GroupIsDeleted),invitation);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteUserNotOwnerTestData))]
    public void Invite_Should_ReturnFailure_WhenSenderIsNotAGroupMember(User target, UserId sender, Group group)
    {
        var invitation = group.Invite(target, sender);

        Assert.True(invitation.IsFailure);
        Assert.NotStrictEqual(Result<Invitation>.Failure(GroupError.UserIsNotMember),invitation);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void AcceptInvitation_Should_ReturnSuccessAddToMembersAndChangeInvitationStatus(User target, UserId sender, Group group)
    {
        var firstInvitation = group.Invite(target, sender);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);
        var result = group.AcceptInvitation(firstInvitation.Value);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Contains(result.Value, group.Members);
        var invitation = group.Invitations.FirstOrDefault(invitation => invitation.Id.Equals(firstInvitation.Value.Id));
        Assert.NotNull(invitation);
        Assert.Equal(InvitationStatus.Accepted, invitation.Status);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void AcceptInvitation_Should_ReturnFailureWhenAlreadyAccepted(User target, UserId sender, Group group)
    {
        var firstInvitation = group.Invite(target, sender);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);
        group.AcceptInvitation(firstInvitation.Value);
        
        var result = group.AcceptInvitation(firstInvitation.Value);

        Assert.True(result.IsFailure);
        Assert.NotStrictEqual(Result<Member>.Failure(GroupError.InvitationAlreadyAccepted), result);
    }
    
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void RejectInvitation_Should_ReturnSuccess(User target, UserId sender, Group group)
    {
        var firstInvitation = group.Invite(target, sender);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);
        var result = group.RejectInvitation(firstInvitation.Value);

        Assert.True(result.IsSuccess);
        var invitation = group.Invitations.FirstOrDefault(invitation => invitation.Id.Equals(firstInvitation.Value.Id));
        Assert.NotNull(invitation);
        Assert.Equal(InvitationStatus.Rejected, invitation.Status);
    }
    
     
    [Theory]
    [ClassData(typeof(GroupInviteProperTestData))]
    public void CancelInvitation_Should_ReturnSuccess(User target, UserId sender, Group group)
    {
        var firstInvitation = group.Invite(target, sender);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);
        var result = group.CancelInvitation(firstInvitation.Value);

        Assert.True(result.IsSuccess);
        var invitation = group.Invitations.FirstOrDefault(invitation => invitation.Id.Equals(firstInvitation.Value.Id));
        Assert.NotNull(invitation);
        Assert.Equal(InvitationStatus.Cancelled, invitation.Status);
    }
    
    [Theory]
    [ClassData(typeof(GroupMembersData))]
    public void RemoveMember_Should_ReturnSuccess(User owner, User basic1, User basic2, Group group)
    {
       
        var memberToDelete = group.Members.FirstOrDefault(member => member.UserId.Equals(basic1.Id));
        Assert.NotNull(memberToDelete);
       var result =  group.RemoveMember(memberToDelete.Id);
       Assert.True(result.IsSuccess);
       Assert.True(!group.Members.Contains(memberToDelete));
    }
    
}

public class GroupInviteProperTestData : TheoryData<User,UserId, Group>
{
    public GroupInviteProperTestData()
    { 
        var userResult = User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var userResult2 = User.Create("Adam","Sandler","a.sandler@o2.pl","secret");
        var user = userResult.Value;
        var target = userResult2.Value;
        Assert.NotNull(user);
        Assert.NotNull(target);
        var group = Group.Create("Kowalscy", user.Id);
        Add(target, user.Id, group);
    }
}


public class GroupInviteUserNotOwnerTestData : TheoryData<User,UserId, Group>
{
    public GroupInviteUserNotOwnerTestData()
    { 
        var userResult = User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var userResult2 = User.Create("Adam","Sandler","a.sandler@o2.pl","secret");
        var userResult3 = User.Create("Denis","Rodman","d.rodman@o2.pl","secret");
        var user = userResult.Value;  
        var otherUser = userResult3.Value;
        var target = userResult2.Value;
        Assert.NotNull(user);
        Assert.NotNull(target);
        var group = Group.Create("Rodmanowie", otherUser.Id);
        Add(target, user.Id, group);
    }
}


public class GroupMembersData : TheoryData<User, User, User, Group>
{
    public GroupMembersData()
    { 
        var userResult = User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var userResult2 = User.Create("Adam","Sandler","a.sandler@o2.pl","secret");
        var userResult3 = User.Create("Denis","Rodman","d.rodman@o2.pl","secret");
        var owner = userResult.Value;  
        var basic1 = userResult3.Value;
        var basic2 = userResult2.Value;
        Assert.NotNull(owner);
        Assert.NotNull(basic1);       
        Assert.NotNull(basic2);

        var group = Group.Create("Rodmanowie", owner.Id);
        var inviteResult = group.Invite(basic1, owner.Id);
        var inviteResult2 = group.Invite(basic2, owner.Id);
        
        Assert.NotNull(inviteResult.Value);
        Assert.NotNull(inviteResult2.Value);      
        
        group.AcceptInvitation(inviteResult.Value);
        group.AcceptInvitation(inviteResult2.Value);
        
        Add(owner, basic1, basic2, group);
    }
}



