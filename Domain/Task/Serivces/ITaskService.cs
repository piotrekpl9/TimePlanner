using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;

namespace Domain.Task.Serivces;

public interface ITaskService
{ 
    //Group can be obtained from user context so there is no need to add additional parameter
    Task<TaskDto> CreateForGroup(CreateTaskDto task);
    Task<TaskDto> Create(CreateTaskDto task);
    Task<TaskDto> Read(TaskId id);
    Task<TaskDto> Update(UpdateTaskDto newTask);
    Task<bool> Delete(TaskId id);

}