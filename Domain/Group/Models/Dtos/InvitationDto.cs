using Domain.Group.Entities;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;

namespace Domain.Group.Models.Dtos;

public class InvitationDto
{
    public InvitationDto(Guid invitationId, string targetEmail, MemberDto creator, InvitationStatus status, DateTime createdAt)
    {
        InvitationId = invitationId;
        TargetEmail = targetEmail;
        Creator = creator;
        Status = status;
        CreatedAt = createdAt;
    }

    public InvitationDto()
    {
        
    }

    public Guid InvitationId { get; private set; }
    public string TargetEmail { get; private set; }
    public MemberDto Creator { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
}