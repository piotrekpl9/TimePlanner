namespace Domain.User.Models;

public class PasswordUpdateRequest(string oldPassword, string newPassword)
{
    public string OldPassword { get;  } = oldPassword; 
    public string NewPassword { get; } = newPassword;
}