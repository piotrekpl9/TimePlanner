namespace Application.Authentication.Model;

public class LoginResponse(string token)
{
    public string Token { get; set; } = token;
}