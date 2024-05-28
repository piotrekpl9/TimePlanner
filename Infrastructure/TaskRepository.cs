using Application.Common.Data;
using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Repositories;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Task.Entities.Task;

namespace Infrastructure;

public class TaskRepository(IApplicationDbContext dbContext, IUnitOfWork unitOfWork) : ITaskRepository
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async System.Threading.Tasks.Task Add(Task task)
    {
        await dbContext.Tasks.AddAsync(task);
    }

    public async Task<Task?> Read(TaskId id)
    {
        return await dbContext.Tasks.Include(task => task.AssignedUsers).Where(task => task.Id.Equals(id)).SingleOrDefaultAsync();
    }

    public async Task<List<Task>> ReadAllByUserId(UserId id)
    {
        return await dbContext.Tasks.Include(task => task.AssignedUsers).Where(task => task.AssignedUsers.Select(user => user.Id).Contains(id)).ToListAsync();
    }

    public async Task<List<Task>> ReadAllByGroupId(GroupId id)
    {
        return await dbContext.Tasks.Include(task => task.AssignedUsers).Where(task => task.GroupId != null && task.GroupId.Equals(id)).ToListAsync();
    }

    public void Update(Task newTask)
    {
        dbContext.Tasks.Update(newTask);
    }

    public async Task<bool> Delete(TaskId id)
    {
        var task = await Read(id);
        if (task is null)
        {
            return false;
        }
        dbContext.Tasks.Remove(task);
        return true;
    }
}