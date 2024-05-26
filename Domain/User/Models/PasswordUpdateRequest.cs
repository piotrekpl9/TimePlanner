namespace Application.Authentication.Model;

public class PasswordUpdateRequest(string oldPassword, string newPassword)
{
    public string OldPassword { get;  } = oldPassword; 
    public string NewPassword { get; } = newPassword;
}