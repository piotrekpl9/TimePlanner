using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Task.Services;

public interface ITaskService
{ 
    Task<Result<TaskDto>> CreateForGroup(CreateTaskDto createDto, UserId userId);
    Task<Result<TaskDto>> Create(CreateTaskDto createDto, UserId userId);
    Task<Result<TaskDto>> Update(TaskId id, UpdateTaskDto newTask);
    Task<bool> Delete(TaskId id);

}