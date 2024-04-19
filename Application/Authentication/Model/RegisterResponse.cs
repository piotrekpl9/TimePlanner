namespace Application.Authentication.Model;


public class RegisterResponse
{
    public RegisterResponse(string token,string name,string surname,string email)
    {
        Token = token;
        Name = name;
        Surname = surname;
        Email = email;
    }

    public string Token { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
}