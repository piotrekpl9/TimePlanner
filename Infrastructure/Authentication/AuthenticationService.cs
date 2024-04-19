using Application.Common.Data;
using Domain.User.Errors;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication;

using Model;
using Common.Interfaces;
using Domain.Shared;
using Domain.User.Repositories;
using Domain.User.Models.Dtos;
using Domain.User.Entities;

public class AuthenticationService(
    IPasswordHasher<User> passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IAuthenticationService
{
    
    public async Task<Result<RegisterResponse>> Register(RegisterRequest userDto)
    {
        try
        {
            var userResult = User.Create(userDto.Name, userDto.Surname, userDto.Email, passwordHasher.HashPassword(null,userDto.Password));

            if (userResult.IsFailure || userResult.Value == null)
            {
                return Result<RegisterResponse>.Failure(userResult.Error);
            }

            var user = userResult.Value!;
            
            await userRepository.Add(user);
            var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Surname);

            await unitOfWork.SaveChangesAsync();
            return Result<RegisterResponse>.Success(new RegisterResponse(token, user.Name, user.Surname, user.Email));
        }
        catch (Exception e)
        {
            return Result<RegisterResponse>.Failure(new Error("error" ,e.Message));
        }
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var user = await userRepository.GetByEmail(loginRequest.Email);
        if (user is null)
        {
            return Result<LoginResponse>.Failure(UserError.DoesntExists);
        }

        var passwordCheck = (passwordHasher.VerifyHashedPassword(
                 null, 
                 user.Password, 
                 loginRequest.Password) == PasswordVerificationResult.Success
                );
        
        if (user.Email != loginRequest.Email || !passwordCheck)
        {
            return Result<LoginResponse>.Failure(UserError.InvalidEmailOrPassword);
        }
        
        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Surname);

        return Result<LoginResponse>.Success(new LoginResponse(token));
    }
}