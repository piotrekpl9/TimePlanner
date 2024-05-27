namespace Domain.Group.Models;

public class InviteUserRequest
{
    
    public InviteUserRequest(Guid groupId,  string email)
    {
        Email = email;
    }

    public string Email { get; }
}