using Domain.Shared;

namespace Application.Authentication.Errors;

    public class AuthenticationError(string name, string code, string description) : Error(name, code, description)
    {
        public static AuthenticationError UserAlreadyDeleted => new AuthenticationError("UserAlreadyDeleted", "AUTH_0", "Cannot delete an already deleted user!");
    }
