using Domain.Shared;

namespace Application.Authentication.Errors;

    public class AuthenticationError(string code, string description) : Error(code, description)
    {
        public static AuthenticationError UserAlreadyDeleted => new AuthenticationError("UserAlreadyDeleted", "Cannot delete an already deleted user!");
    }
