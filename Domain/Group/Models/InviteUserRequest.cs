namespace Domain.Group.Models;

public class InviteUserRequest
{
    public InviteUserRequest(string email)
    {
        Email = email;
    }

    public string Email { get; }
}