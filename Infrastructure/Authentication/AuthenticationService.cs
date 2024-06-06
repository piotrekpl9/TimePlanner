using Application.Authentication;
using Application.Authentication.Model;
using Application.Common.Data;
using Application.Common.Interfaces;
using Domain.Shared;
using Domain.User.Errors;
using Domain.User.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication;

public class AuthenticationService(
    IPasswordHasher<Domain.User.Entities.User> passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IAuthenticationService
{
    
    public async Task<Result<RegisterResultDto>> Register(RegisterUserDto userDto)
    {
        try
        {
            var user = Domain.User.Entities.User.Create(userDto.Name, userDto.Surname, userDto.Email, passwordHasher.HashPassword(null,userDto.Password));

            await userRepository.Add(user);
            var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Surname);

            await unitOfWork.SaveChangesAsync();
            return Result<RegisterResultDto>.Success(new RegisterResultDto(user.Name, user.Surname, user.Email));
        }
        catch (Exception e)
        {
            return Result<RegisterResultDto>.Failure(new Error("AuthError" , "AUTH_X",e.Message));
        }
    }

    public async Task<Result<LoginResultDto>> Login(LoginUserDto loginRequest)
    {
        var user = await userRepository.GetByEmail(loginRequest.Email);
        if (user is null)
        {
            return Result<LoginResultDto>.Failure(UserError.DoesntExists);
        }

        var passwordCheck = (passwordHasher.VerifyHashedPassword(
                 null, 
                 user.Password, 
                 loginRequest.Password) == PasswordVerificationResult.Success
                );
        
        if (user.Email != loginRequest.Email || !passwordCheck)
        {
            return Result<LoginResultDto>.Failure(UserError.InvalidEmailOrPassword);
        }
        
        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Surname);

        return Result<LoginResultDto>.Success(new LoginResultDto(token, user.Id.Value));
    }
}