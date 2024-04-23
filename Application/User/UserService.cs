using Application.Authentication.Model;
using Application.Common.Data;
using Domain.Shared;
using Domain.User.Errors;
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
    public Task<List<UserDto>> ReadAllByCurrentGroup()
    {
        throw new NotImplementedException();
    }

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

    public async Task<Result> UpdatePassword(UserId id, PasswordUpdateRequest passwordUpdateRequest)
    {
        var user = await userRepository.GetById(id);
        if (user is null)
        {
            return Result.Failure(UserError.DoesntExists);
        }

        user.Password = passwordHasher.HashPassword(null, passwordUpdateRequest.NewPassword);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

}