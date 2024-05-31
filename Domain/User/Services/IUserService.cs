using Domain.Shared;
using Domain.User.Models;
using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;

namespace Domain.User.Services;

public interface IUserService
{
    Task<Result> Delete(UserId id);
    Task<Result> UpdatePassword(UserId id, UpdatePasswordDto updatePasswordDto);
}