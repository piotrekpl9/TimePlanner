using Domain.Shared;

namespace Domain.User.Errors;

public class UserError(string name, string code, string description) : Error(name, code, description)
{
    public static UserError AlreadyDeleted => new UserError("UserAlreadyDeleted", "USER_1","Cannot delete an already deleted user!");
    public static UserError AlreadyExists => new UserError("UserAlreadyExists", "USER_2","Cannot add user with given email because one already exists!");
    public static UserError DoesntExists => new UserError("UserDoesntExists", "USER_3","User doesn't exists!");
    public static UserError IdDoesntMatch => new UserError("IdDoesntMatch", "USER_4","Id doesnt match!");
    public static UserError InvalidEmailOrPassword => new UserError("BadEmailOrPassword", "USER_5","Given email or password are invalid!");
    public static UserError NameEmpty => new UserError("UserNameEmpty", "USER_6","Name cannot be empty!");
    public static UserError SurnameEmpty => new UserError("UserSurnameEmpty", "USER_7","Surname cannot be empty!");
    public static UserError EmailEmpty => new UserError("UserEmailEmpty", "USER_8","Email cannot be empty!");
    public static UserError PasswordToShort => new UserError("UserPasswordToShort", "USER_9","Password is to short! Minimum password length is 6.");
    public static UserError PasswordToLong => new UserError("UserPasswordToLong", "USER_10","Password is to long! Maximum password length is 30.");
    //PASSWORD UPDATE ERRORS
    public static UserError PasswordsIdentical => new UserError("PasswordsIdentical", "USER_11","New password has to be different than old one!");
    public static UserError BadOldPassword => new UserError("BadOldPassword", "USER_12","Provided old password is invalid!");
    public static UserError OldPasswordEmpty => new UserError("OldPasswordEmpty", "USER_13","You have to provide old password!");
    public static UserError NewPasswordEmpty => new UserError("NewPasswordEmpty", "USER_14","You have to provide new password!");
    public static UserError GuidNotProvided => new UserError("GuidNotProvided", "USER_15","You have to provide user id!");

}