using Domain.Shared;
using Domain.User.Errors;
using Domain.User.Models.Dtos;
using Domain.User.Repositories;
using Domain.User.Services;
using Domain.User.ValueObjects;

namespace Application.User;
using Domain.User.Entities;

public class UserService : IUserService
{
   


    public Task<List<UserDto>> ReadAllByCurrentGroup()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(UserId id)
    {
        throw new NotImplementedException();
    }
}