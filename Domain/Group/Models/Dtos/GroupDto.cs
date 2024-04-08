using Domain.Group.Entities;

namespace Domain.Group.Models.Dtos;

public class GroupDto
{
    public GroupDto(List<Invitation> invitations, List<Member> members, string name, DateTime createdAt, Member creator)
    {
        Invitations = invitations;
        Members = members;
        Name = name;
        CreatedAt = createdAt;
        Creator = creator;
    }

    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Invitation> Invitations { get; set; }
    private List<Member> Members { get; set; }
    public Member Creator { get; set; }
}