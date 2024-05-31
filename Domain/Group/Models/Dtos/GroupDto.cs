using Domain.Group.Entities;
using Domain.Group.Models.ValueObjects;

namespace Domain.Group.Models.Dtos;

public class GroupDto
{
    public GroupDto(Guid groupId ,List<InvitationDto> invitations,  List<MemberDto> members, string name, DateTime createdAt, MemberDto creator)
    {
        GroupId = groupId;
        Invitations = invitations;
        Members = members;
        Name = name;
        CreatedAt = createdAt;
        Creator = creator;
    }
    public GroupDto()
    {
     
    }

    public Guid GroupId { get; private set; } 
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<MemberDto> Members { get; private set; }
    public List<InvitationDto> Invitations { get; private set; }
    public MemberDto Creator { get; private set; }
}