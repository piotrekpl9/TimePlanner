using Application.Common.Data;
using Domain.Shared;
using Domain.User.Errors;
using Domain.User.Models.Dtos;
using Domain.User.Repositories;
using Domain.User.Services;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.User;
using Domain.User.Entities;

public class UserService(
    IPasswordHasher<User> passwordHasher,
    IUserRepository userRepository,
    ILogger<UserService> logger,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result> Delete(UserId id)
    {
        var user = await userRepository.GetById(id);
        if (user is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} Search criteria was taskId: {id}");
            return Result.Failure(UserError.DoesntExists);
        }

        var result = user.Delete();

        if (result.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{result.Error.Description} Operation: Delete");
            return result;
        }
        
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> UpdatePassword(UserId id, UpdatePasswordDto updatePasswordDto)
    {
        var user = await userRepository.GetById(id);
        if (user is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} Search criteria was taskId: {id}");
            return Result.Failure(UserError.DoesntExists);
        } 
        var oldPasswordCheck = passwordHasher.VerifyHashedPassword(null, user.Password, updatePasswordDto.OldPassword) ==
                 PasswordVerificationResult.Success;
        if (!oldPasswordCheck)
        {
            logger.Log(LogLevel.Error, $"{UserError.BadOldPassword.Description}");
            return Result.Failure(UserError.BadOldPassword);
        }
        var newPasswordCheck =  passwordHasher.VerifyHashedPassword(null, user.Password, updatePasswordDto.NewPassword) !=
                                PasswordVerificationResult.Success;
        if (!newPasswordCheck)
        {
            logger.Log(LogLevel.Error, $"{UserError.PasswordsIdentical.Description}");
            return Result.Failure(UserError.PasswordsIdentical);
        }
        user.SetPassword(passwordHasher.HashPassword(null, updatePasswordDto.NewPassword));
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

}