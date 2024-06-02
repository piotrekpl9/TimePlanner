namespace Presentation.Model.Requests;

public class InviteUserRequest
{
    public InviteUserRequest(string email)
    {
        Email = email;
    }

    
    public string Email { get; set; }
}