using Domain.Shared;

namespace Domain.Group.Errors;

public class GroupError(string code, string description) : Error(code, description)
{
    public static GroupError UserAlreadyInvited => new GroupError("UserAlreadyInvited", "User with given id has already been invited!");
    public static GroupError UserIsAMember => new GroupError("UserIsAMember", "User with given id is already member of this group!");
    public static GroupError InvitationAlreadyAccepted => new GroupError("InvitationAlreadyAccepted", "Invitation has already been accepted!");
    public static GroupError InvitationAlreadyExpired => new GroupError("InvitationAlreadyExpired", "Invitation is already expired!");
    public static GroupError InvitationAlreadyRejected => new GroupError("InvitationAlreadyRejected", "Invitation is already rejected!");
    public static GroupError InvitationAlreadyCancelled => new GroupError("InvitationAlreadyCancelled", "Invitation is already cancelled!");
    public static GroupError GroupAlreadyDeleted => new GroupError("GroupAlreadyDeleted", "Group is already deleted!");
    public static GroupError GroupIsDeleted => new GroupError("GroupIsDeleted", "Group is deleted!");
    public static GroupError GroupNotFound => new GroupError("GroupNotFound", "Group not found!");
    public static GroupError InvitationNotFound => new GroupError("InvitationNotFound", "Invitation not found!");
    public static GroupError UserIsNotMember => new GroupError("UserIsNotMember", "User is not a member of given group!");
    public static GroupError MemberNotFound => new GroupError("MemberNotFound", "Member not found!");
    public static GroupError FailedToRemoveMember => new GroupError("FailedToRemoveMember", "Failed to remove member!");
    public static GroupError UserIsNotInvitationTarget => new GroupError("UserIsNotInvitationTarget", "User is not invitation target!");
    public static GroupError UserIsNotInvitationOwner => new GroupError("UserIsNotInvitationOwner", "User is not invitation owner!");
    public static GroupError UserInvitingItself => new GroupError("UserInvitingItself", "User is trying to invite itself!");
    public static GroupError UserIsNotGroupOwner => new GroupError("UserIsNotGroupOwner", "User is not group owner!");
    public static GroupError UserAlreadyInOtherGroup => new GroupError("UserAlreadyInOtherGroup", "User is already other group member!");
}