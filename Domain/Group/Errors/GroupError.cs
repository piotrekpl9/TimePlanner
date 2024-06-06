using Domain.Shared;

namespace Domain.Group.Errors;

public class GroupError(string name, string code, string description) : Error(name, code, description)
{
    public static GroupError UserAlreadyInvited => new GroupError("UserAlreadyInvited", "GROUP_1","User with given id has already been invited!");
    public static GroupError UserIsAMember => new GroupError("UserIsAMember", "GROUP_2","User with given id is already member of this group!");
    public static GroupError InvitationAlreadyAccepted => new GroupError("InvitationAlreadyAccepted", "GROUP_3","Invitation has already been accepted!");
    public static GroupError InvitationAlreadyExpired => new GroupError("InvitationAlreadyExpired", "GROUP_4","Invitation is already expired!");
    public static GroupError InvitationAlreadyRejected => new GroupError("InvitationAlreadyRejected", "GROUP_5","Invitation is already rejected!");
    public static GroupError InvitationAlreadyCancelled => new GroupError("InvitationAlreadyCancelled", "GROUP_6","Invitation is already cancelled!");
    public static GroupError GroupAlreadyDeleted => new GroupError("GroupAlreadyDeleted","GROUP_7", "Group is already deleted!");
    public static GroupError GroupIsDeleted => new GroupError("GroupIsDeleted", "GROUP_8","Group is deleted!");
    public static GroupError GroupNotFound => new GroupError("GroupNotFound", "GROUP_9","Group not found!");
    public static GroupError InvitationNotFound => new GroupError("InvitationNotFound", "GROUP_10","Invitation not found!");
    public static GroupError UserIsNotMember => new GroupError("UserIsNotMember", "GROUP_11","User is not a member of given group!");
    public static GroupError MemberNotFound => new GroupError("MemberNotFound", "GROUP_12","Member not found!");
    public static GroupError FailedToRemoveMember => new GroupError("FailedToRemoveMember", "GROUP_13","Failed to remove member!");
    public static GroupError UserIsNotInvitationTarget => new GroupError("UserIsNotInvitationTarget", "GROUP_14","User is not invitation target!");
    public static GroupError UserIsNotInvitationOwner => new GroupError("UserIsNotInvitationOwner", "GROUP_15","User is not invitation owner!");
    public static GroupError UserInvitingItself => new GroupError("UserInvitingItself", "GROUP_16","User is trying to invite itself!");
    public static GroupError UserIsNotGroupOwner => new GroupError("UserIsNotGroupOwner", "GROUP_17","User is not group owner!");
    public static GroupError UserAlreadyInOtherGroup => new GroupError("UserAlreadyInOtherGroup", "GROUP_18","User is already other group member!");
}