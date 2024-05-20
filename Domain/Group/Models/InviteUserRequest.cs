namespace Domain.Group.Models;

public class InviteUserRequest
{
    
    public InviteUserRequest(Guid groupId,  string email)
    {
        GroupId = groupId;
        Email = email;
    }

    public string Email { get; }
    public Guid GroupId { get; }
}