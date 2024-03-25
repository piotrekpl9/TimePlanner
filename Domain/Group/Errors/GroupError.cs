using Domain.Shared;

namespace Domain.Group.Errors;

public class GroupError(string code, string description) : Error(code, description)
{
    public static GroupError UserAlreadyInvited => new GroupError("UserAlreadyInvited", "User with given id has already been invited!");
    public static GroupError UserIsAMember => new GroupError("UserIsAMember", "User with given id is already member of this group!");
}