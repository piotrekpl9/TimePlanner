namespace Domain.User.Models.Dtos;

public class UpdatePasswordDto(string oldPassword, string newPassword)
{
    public string OldPassword { get;  } = oldPassword; 
    public string NewPassword { get; } = newPassword;
}