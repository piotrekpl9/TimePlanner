using Domain.User.Errors;

namespace Application.Authentication;

using Application.Authentication.Model;
using Application.Common.Interfaces;
using Domain.Shared;
using Domain.User.Repositories;
using Domain.User.Services;
using Domain.User.ValueObjects;
using Domain.User.Models.Dtos;
using Domain.User.Entities;
public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<RegisterResponse>> Register(CreateUserDto userDto)
    {
        var existingUser = await _userRepository.GetByEmail(userDto.Email);
        if (existingUser is not null)
        {
            return Result<RegisterResponse>.Failure(UserError.UserAlreadyExists);
        }
        
        User user = User.Create(userDto.Name,userDto.Surname,userDto.Email,userDto.Password);
        
        var result = await _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(result.Id, result.Name, result.Surname);

        return Result<RegisterResponse>.Success(new RegisterResponse(token, result.Name, result.Surname,result.Email));
    }

    public RegisterResponse Login(string email, string password)
    {
        throw new NotImplementedException();
    }
}