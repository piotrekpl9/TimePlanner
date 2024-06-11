using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Task.Repositories;

public interface ITaskRepository
{
    public System.Threading.Tasks.Task Add(Entities.Task task);
    public Task<Entities.Task?> Get(TaskId id);
    public Task<List<Entities.Task>> GetAllByUserId(UserId id);
    public Task<List<Entities.Task>> GetAllByGroupId(GroupId id);
    public void Update(Entities.Task newTask);
    public Task<bool> Delete(TaskId id);
}