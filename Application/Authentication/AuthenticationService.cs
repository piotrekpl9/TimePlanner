using Application.Common.Interfaces;
using Domain.User.Entities;
using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;

namespace Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public AuthenticationResult Register(CreateUserDto userRequest)
    {

        User user = User.Create(userRequest.Name,userRequest.Surname,userRequest.Email,userRequest.Password);
        
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Surname);
        
        
        return new AuthenticationResult(token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        throw new NotImplementedException();
    }
}