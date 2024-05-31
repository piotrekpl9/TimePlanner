using Application.Authentication.Model;
using Domain.Shared;
using Domain.User.Models.Dtos;

namespace Application.Authentication;

public interface IAuthenticationService
{
    Task<Result<RegisterResultDto>> Register(RegisterUserDto userDto);
    Task<Result<LoginResultDto>> Login(LoginUserDto loginRequest);
}