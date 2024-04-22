using Application.Authentication.Model;
using Domain.Shared;
using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;

namespace Domain.User.Services;

public interface IUserService
{
    Task<List<UserDto>> ReadAllByCurrentGroup();
    Task<bool> Delete(UserId id);
    Task<Result> UpdatePassword(UserId id, PasswordUpdateRequest passwordUpdateRequest);
}