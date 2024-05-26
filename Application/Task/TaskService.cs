using Application.Common.Data;
using Domain.Group.Errors;
using Domain.Group.Repositories;
using Domain.Group.Services;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Repositories;
using Domain.Task.Serivces;
using Domain.Task.TaskErrors;
using Domain.User.Errors;
using Domain.User.Models.Dtos;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;

namespace Application.Task;
using Task = Domain.Task.Entities.Task;

public class TaskService(ITaskRepository taskRepository, IGroupRepository groupRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : ITaskService
{
    public async Task<Result<TaskDto>> CreateForGroup(CreateTaskRequest createRequest, UserId userId)
    {
        var creator = await userRepository.GetById(userId);
        if (creator is null)
        {
            return Result<TaskDto>.Failure(UserError.AlreadyExists);
        }

        var group = await groupRepository.ReadGroupByUserId(userId);
        if (group is null)
        {
            return Result<TaskDto>.Failure(GroupError.GroupNotFound);
        }
        
        var usersList = await userRepository.GetByIdList(group.Members.Select(member => member.UserId).ToList());
        
        var task = Task.CreateForGroup(createRequest.Name, createRequest.Notes, TaskStatus.Pending, usersList, group.Id, creator, createRequest.PlannedAt);

        await taskRepository.Add(task);
        await unitOfWork.SaveChangesAsync();
        
        var userDto = new UserDto(task.Creator.Name, task.Creator.Surname, task.Creator.Email, task.Creator.CreatedAt);
        var taskDto = new TaskDto(task.Name, task.Notes, task.Status.ToString(), userDto, task.GroupId?.Value, task.PlannedAt, task.CreatedAt);
        
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<Result<TaskDto>> Create(CreateTaskRequest createRequest, UserId userId)
    {
        var creator = await userRepository.GetById(userId);
        if (creator is null)
        {
            return Result<TaskDto>.Failure(UserError.DoesntExists);
        }
        
        var task = Task.CreateForSelf(createRequest.Name, createRequest.Notes, TaskStatus.Pending, creator, createRequest.PlannedAt.ToUniversalTime());

        await taskRepository.Add(task);
        await unitOfWork.SaveChangesAsync();
        var userDto = new UserDto(task.Creator.Name, task.Creator.Surname, task.Creator.Email, task.Creator.CreatedAt);
        var taskDto = new TaskDto(task.Name, task.Notes, task.Status.ToString(), userDto, task.GroupId?.Value, task.PlannedAt, task.CreatedAt);
        
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<Result<TaskDto>> Read(TaskId id, UserId userId)
    {
        var task = await taskRepository.Read(id);
        if (task is null)
        {
            return Result<TaskDto>.Failure(TaskError.TaskNotFound);
        }
        
        if(!task.AssignedUsers.Any(user => user.Id.Equals(userId)))
        {
            return Result<TaskDto>.Failure(TaskError.TaskIsNotAssignedToUser);
        }

        var userDto = new UserDto(task.Creator.Name, task.Creator.Surname, task.Creator.Email, task.Creator.CreatedAt);
        var taskDto = new TaskDto(task.Name, task.Notes, task.Status.ToString(), userDto, task.GroupId?.Value, task.PlannedAt, task.CreatedAt);
        
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<Result<TaskDto>> Update(TaskId id, UpdateTaskRequest newTask, UserId userId)
    {
        var task = await taskRepository.Read(id);
        if (task is null)
        {
            return Result<TaskDto>.Failure(TaskError.TaskNotFound);
        }
        
        if(!task.AssignedUsers.Any(user => user.Id.Equals(userId)))
        {
            return Result<TaskDto>.Failure(TaskError.TaskIsNotAssignedToUser);
        }
        
        task.Update(newTask.Name, newTask.Notes, newTask.Status, newTask.PlannedAt);
        taskRepository.Update(task);
        await unitOfWork.SaveChangesAsync();
        
        var userDto = new UserDto(task.Creator.Name, task.Creator.Surname, task.Creator.Email, task.Creator.CreatedAt);
        var taskDto = new TaskDto(task.Name, task.Notes, task.Status.ToString(), userDto, task.GroupId?.Value, task.PlannedAt, task.CreatedAt);
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<bool> Delete(TaskId id, UserId userId)
    {
        var task = await taskRepository.Read(id);
        if (task is null)
        {
            return false;
        }
        
        if(!task.AssignedUsers.Any(user => user.Id.Equals(userId)))
        {
            return false;
        }

        var result = task.Delete();
        if(result.IsFailure)
        {
            return false;
        }
        
        taskRepository.Update(task);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}