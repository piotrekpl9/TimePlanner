using Domain.Group.Entities;
using Domain.Group.Models.Enums;

namespace Domain.Group.Models.Dtos;

public class InvitationDto
{
    public InvitationDto(string targetEmail, Member creator, InvitationStatus status, DateTime createdAt)
    {
        TargetEmail = targetEmail;
        Creator = creator;
        Status = status;
        CreatedAt = createdAt;
    }

    public string TargetEmail { get; private set; }
    public Member Creator { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
}