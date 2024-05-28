using Domain.Group.Models.ValueObjects;
using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;

namespace Domain.User.Repositories;
using Entities;
using System.Threading.Tasks;
public interface IUserRepository
{
    Task Add(User user);
    Task<User?> GetById(UserId id);
    Task<List<User>> GetByIdList(List<UserId> idList);
    Task<User?> GetByEmail(string email);
    Task<List<User>> GetAllByGroupId(GroupId groupId);
    void Update(User user);
    Task<bool> Delete(UserId id);
}