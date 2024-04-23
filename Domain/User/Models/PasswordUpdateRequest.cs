namespace Application.Authentication.Model;

public class PasswordUpdateRequest(string oldPassword, string newPassword, Guid guid)
{
    public Guid Guid { get; } = guid; 
    public string OldPassword { get;  } = oldPassword; 
    public string NewPassword { get; } = newPassword;
}