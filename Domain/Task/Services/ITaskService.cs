using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Task.Services;

public interface ITaskService
{ 
    Task<Result<TaskDto>> CreateForGroup(CreateTaskRequest createRequest, UserId userId);
    Task<Result<TaskDto>> Create(CreateTaskRequest createRequest, UserId userId);
    Task<Result<TaskDto>> Update(TaskId id, UpdateTaskRequest newTask);
    Task<bool> Delete(TaskId id);

}