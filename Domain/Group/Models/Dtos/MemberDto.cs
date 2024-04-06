using Domain.Group.Models.Enums;

namespace Domain.Group.Models.Dtos;

public class MemberDto
{
    public MemberDto(string name, Role role, DateTime createdAt)
    {
        Name = name;
        Role = role;
        CreatedAt = createdAt;
    }
    public string Name { get; set; }
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
}