namespace Presentation.Model.Requests;

public class LoginRequest(string email, string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}