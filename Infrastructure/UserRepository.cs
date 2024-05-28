using Microsoft.EntityFrameworkCore;

namespace Infrastructure.User;

using Application.Common.Data;
using Domain.Group.Models.ValueObjects;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Domain.User.Entities;

public class UserRepository(IApplicationDbContext dbContext, IUnitOfWork unitOfWork) : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }

    public Task<User?> GetById(UserId id)
    {
        return dbContext.Users.SingleOrDefaultAsync(user => user.Id.Equals(id));
    }

    public async Task<List<User>> GetByIdList(List<UserId> idList)
    {
        return await dbContext.Users.Where(user => idList.Contains(user.Id)).ToListAsync();
    }
    
    public Task<User?> GetByEmail(string email)
    {
        return dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
    }
    public  Task<List<User>> GetAllByGroupId(GroupId groupId)
    {
        throw new NotImplementedException();
    }

    public void Update(User user)
    {
         dbContext.Users.Update(user);
    }

    public async Task<bool> Delete(UserId id)
    {
        var user = await GetById(id);
        if (user is null)
        {
            return false;
        }
        dbContext.Users.Remove(user);
        return true;
    }
}