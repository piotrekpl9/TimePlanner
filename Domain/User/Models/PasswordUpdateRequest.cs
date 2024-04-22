namespace Application.Authentication.Model;

public class PasswordUpdateRequest(string oldPassword, string newPassword)
{
    public string OldPassword { get; set; } = oldPassword; 
    public string NewPassword { get; set; } = newPassword;
}