namespace Application.Authentication.Model;

public class LoginResultDto(string token, Guid userId)
{
    public Guid UserId { get; set; } = userId;
    public string Token { get; set; } = token;
}