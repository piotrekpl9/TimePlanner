namespace Application.Authentication.Model;


public class RegisterResponse(string name, string surname, string email)
{
    public string Name { get; set; } = name;
    public string Surname { get; set; } = surname;
    public string Email { get; set; } = email;
}