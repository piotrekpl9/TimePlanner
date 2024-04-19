using Domain.Shared;

namespace Domain.User.Errors;

public class UserError(string code, string description) : Error(code, description)
{
    public static UserError AlreadyDeleted => new UserError("UserAlreadyDeleted", "Cannot delete an already deleted user!");
    public static UserError AlreadyExists => new UserError("UserAlreadyExists", "Cannot add user with given email because one already exists!");
    public static UserError DoesntExists => new UserError("UserDoesntExists", "User doesn't exists!");
    public static UserError InvalidEmailOrPassword => new UserError("BadEmailOrPassword", "Given email or password are invalid!");
    public static UserError NameEmpty => new UserError("UserNameEmpty", "Name cannot be empty!");
    public static UserError SurnameEmpty => new UserError("UserSurnameEmpty", "Surname cannot be empty!");
    public static UserError EmailEmpty => new UserError("UserEmailEmpty", "Email cannot be empty!");
    public static UserError PasswordToShort => new UserError("UserPasswordToShort", "Password is to short! Minimum password length is 6.");
    public static UserError PasswordToLong => new UserError("UserPasswordToLong", "Password is to long! Maximum password length is 30.");
}