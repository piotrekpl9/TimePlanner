using Domain.Group.Models.Enums;

namespace Domain.Group.Models.Dtos;

public class MemberDto
{
    public MemberDto(string name, string surname, Role role, DateTime createdAt)
    {
        Name = name;
        Surname = surname;
        Role = role;
        CreatedAt = createdAt;
    }

    public MemberDto()
    {
        
    }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Role Role { get;  set; }
    public DateTime CreatedAt { get;  set; }
}