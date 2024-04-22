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
    IPasswordHasher<Domain.User.Entities.User> passwordHasher,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    public Task<List<UserDto>> ReadAllByCurrentGroup()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(UserId id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdatePassword(UserId id, PasswordUpdateRequest passwordUpdateRequest)
    {
        throw new NotImplementedException();
    }

}