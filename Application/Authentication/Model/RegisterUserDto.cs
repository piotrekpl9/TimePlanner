namespace Application.Authentication.Model;

public class RegisterUserDto(string name, string surname, string email, string password)
{
    public string Name { get; set; } = name;
    public string Surname { get; set; } = surname;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}