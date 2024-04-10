using Microsoft.EntityFrameworkCore;

namespace Infrastructure.User;

using Application.Common.Data;
using Domain.Group.Models.ValueObjects;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Domain.User.Entities;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public Task<User?> GetById(UserId id)
    {
        return _dbContext.Users.SingleOrDefaultAsync(user => user.Id.Equals(id));
    }

    public Task<User?> GetByEmail(string email)
    {
        return _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
    }
    //TODO ????????
    public Task<List<User>> GetAllByGroupId(GroupId groupId)
    {
        return _dbContext.Users.SingleOrDefaultAsync(user => user..Equals(id));

    }

    public Task<User> Update(User user)
    {
        return _dbContext.Users.Update(user);
    }

    public Task<bool> Delete(UserId id)
    {
        throw new NotImplementedException();
    }
}