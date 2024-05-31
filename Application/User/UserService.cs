using Application.Authentication.Model;
using Application.Common.Data;
using Domain.Shared;
using Domain.User.Errors;
using Domain.User.Models;
using Domain.User.Models.Dtos;
using Domain.User.Repositories;
using Domain.User.Services;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Application.User;
using Domain.User.Entities;

public class UserService(
    IPasswordHasher<User> passwordHasher,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result> Delete(UserId id)
    {
        var user = await userRepository.GetById(id);
        if (user is null)
        {
            return Result.Failure(UserError.DoesntExists);
        }

        var res = user.Delete();

        if (res.IsFailure)
        {
            return res;
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
            return Result.Failure(UserError.DoesntExists);
        } 
        var oldPasswordCheck = passwordHasher.VerifyHashedPassword(null, user.Password, updatePasswordDto.OldPassword) ==
                 PasswordVerificationResult.Success;
        if (!oldPasswordCheck)
        {
            return Result.Failure(UserError.BadOldPassword);
        }
        var newPasswordCheck =  passwordHasher.VerifyHashedPassword(null, user.Password, updatePasswordDto.NewPassword) !=
                                PasswordVerificationResult.Success;
        if (!newPasswordCheck)
        {
            return Result.Failure(UserError.PasswordsIdentical);
        }
        user.SetPassword(passwordHasher.HashPassword(null, updatePasswordDto.NewPassword));
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

}