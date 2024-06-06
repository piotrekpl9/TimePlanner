using Domain.Group.Models.Enums;

namespace Domain.Group.Models.Dtos;

public class MemberDto
{
    public MemberDto(Guid memberId,string name, string surname, Role role, DateTime createdAt)
    {
        MemberId = memberId;
        Name = name;
        Surname = surname;
        Role = role;
        CreatedAt = createdAt;
    }

    public MemberDto()
    {
        
    }

    public Guid MemberId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Role Role { get;  set; }
    public DateTime CreatedAt { get;  set; }
}