namespace Application.Authentication.Model;

public class LoginUserDto(string email, string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}