using Domain.Shared;

namespace Domain.User.Errors;

public class UserErrors(string code, string description) : Error(code, description)
{
    public static UserErrors UserAlreadyDeleted => new UserErrors("UserAlreadyDeleted", "Cannot delete an already deleted user!");
}