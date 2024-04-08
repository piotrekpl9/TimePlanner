namespace Domain.User.Models.Dtos;

public class UserDto
{
    public UserDto(string name, string surname, string email, DateTime createdAt)
    {
        Name = name;
        Surname = surname;
        Email = email;
        CreatedAt = createdAt;
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
}