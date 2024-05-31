namespace Domain.Group.Models.Dtos;

public class InviteUserDto(string email)
{
    public string Email { get; } = email;
}