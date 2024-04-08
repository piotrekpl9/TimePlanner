using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Task.Repositories;

public interface ITaskRepository
{
    public Task<Entities.Task> Create(Entities.Task task);
    public Task<Entities.Task> Read(TaskId id);
    public Task<List<Entities.Task>> ReadAllByUserId(UserId id);
    public Task<List<Entities.Task>> ReadAllByGroupId(GroupId id);
    public Task<Entities.Task> Update(Entities.Task newTask);
    public Task<bool> Delete(TaskId id);
}