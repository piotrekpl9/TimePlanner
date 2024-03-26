using Domain.Shared;

namespace Domain.Group.Errors;

public class GroupError(string code, string description) : Error(code, description)
{
    public static GroupError UserAlreadyInvited => new GroupError("UserAlreadyInvited", "User with given id has already been invited!");
    public static GroupError UserIsAMember => new GroupError("UserIsAMember", "User with given id is already member of this group!");
    public static GroupError InvitationAlreadyAccepted => new GroupError("InvitationAlreadyAccepted", "Invitation has already been accepted!");
    public static GroupError InvitationAlreadyExpired=> new GroupError("InvitationAlreadyExpired", "Invitation is already expired!");
    public static GroupError InvitationAlreadyRejected=> new GroupError("InvitationAlreadyRejected", "Invitation is already Rejected!");
    public static GroupError GroupAlreadyDeleted=> new GroupError("GroupAlreadyDeleted", "Group is already deleted!");

}