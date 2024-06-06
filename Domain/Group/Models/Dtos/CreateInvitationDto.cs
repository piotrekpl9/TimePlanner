namespace Domain.Group.Models.Dtos;

public class CreateInvitationDto(string email)
{
    public string Email { get; } = email;
}