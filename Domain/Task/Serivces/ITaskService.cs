using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Task.Serivces;

public interface ITaskService
{ 
    //Group can be obtained from user context so there is no need to add additional parameter
    Task<Result<TaskDto>> CreateForGroup(CreateTaskRequest createRequest, UserId userId);
    Task<Result<TaskDto>> Create(CreateTaskRequest createRequest, UserId userId);
    Task<Result<TaskDto>> Read(TaskId id, UserId userId);
    Task<Result<TaskDto>> Update(TaskId id, UpdateTaskRequest newTask, UserId userId);
    Task<bool> Delete(TaskId id, UserId userId);

}