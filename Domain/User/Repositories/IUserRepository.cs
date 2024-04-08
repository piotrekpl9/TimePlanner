using Domain.Group.Models.ValueObjects;
using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;

namespace Domain.User.Repositories;
using Entities;
 
public interface IUserRepository
{
    Task<User> Create(CreateUserDto userDto);
    Task<User> Read(UserId groupId);
    Task<List<User>> ReadAllByGroupId(GroupId groupId);
    Task<User> Update(User user);
    Task<bool> Delete(UserId id);
}