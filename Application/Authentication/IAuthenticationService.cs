using Domain.User.Models.Dtos;

namespace Application.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Register(CreateUserDto user);
    AuthenticationResult Login(string email, string password);
}