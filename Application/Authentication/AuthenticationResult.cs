using Domain.User.Entities;

namespace Application.Authentication;

public class AuthenticationResult
{
    public AuthenticationResult(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}