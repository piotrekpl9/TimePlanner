using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;

namespace Domain.User.Services;

public interface IUserService
{
    Task<UserDto> Create(CreateUserDto user);
    Task<List<UserDto>> ReadAllByCurrentGroup();
    Task<bool> Delete(UserId id);
}