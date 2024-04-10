using Application.Authentication.Model;
using Domain.Shared;
using Domain.User.Models.Dtos;

namespace Application.Authentication;

public interface IAuthenticationService
{
    Task<Result<RegisterResponse>> Register(CreateUserDto userDto);
    RegisterResponse Login(string email, string password);
}