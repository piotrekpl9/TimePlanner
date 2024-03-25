using Domain.Shared;

namespace Domain.User.Errors;

public class UserError(string code, string description) : Error(code, description)
{
    public static UserError UserAlreadyDeleted => new UserError("UserAlreadyDeleted", "Cannot delete an already deleted user!");
}