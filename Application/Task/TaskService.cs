using Application.Common.Data;
using AutoMapper;
using Domain.Group.Errors;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.Group.Services;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Repositories;
using Domain.Task.Services;
using Domain.Task.TaskErrors;
using Domain.User.Errors;
using Domain.User.Models.Dtos;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;

namespace Application.Task;
using Task = Domain.Task.Entities.Task;

public class TaskService(ITaskRepository taskRepository, IGroupRepository groupRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper) : ITaskService
{
    public async Task<Result<TaskDto>> CreateForGroup(CreateTaskDto createDto, UserId userId)
    {
        var creator = await userRepository.GetById(userId);
        if (creator is null)
        {
            return Result<TaskDto>.Failure(UserError.DoesntExists);
        }

        var group = await groupRepository.ReadGroupByUserId(userId);
        if (group is null)
        {
            return Result<TaskDto>.Failure(GroupError.GroupNotFound);
        }
        
        var usersList = await userRepository.GetByIdList(group.Members.Select(member => member.User.Id).ToList());
        
        var task = Task.CreateForGroup(createDto.Name, createDto.Notes, TaskStatus.NotStarted, usersList, group.Id, creator, createDto.PlannedStartHour.ToUniversalTime(),createDto.PlannedEndHour.ToUniversalTime());

        await taskRepository.Add(task);
        await unitOfWork.SaveChangesAsync();
        
        var taskDto = mapper.Map<TaskDto>(task);
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<Result<TaskDto>> Create(CreateTaskDto createDto, UserId userId)
    {
        var creator = await userRepository.GetById(userId);
        if (creator is null)
        {
            return Result<TaskDto>.Failure(UserError.DoesntExists);
        }
        
        var task = Task.CreateForSelf(createDto.Name, createDto.Notes, TaskStatus.NotStarted, creator, createDto.PlannedStartHour.ToUniversalTime(),createDto.PlannedEndHour.ToUniversalTime());

        await taskRepository.Add(task);
        await unitOfWork.SaveChangesAsync();
        
        var taskDto = mapper.Map<TaskDto>(task);

        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<Result<TaskDto>> Update(TaskId id, UpdateTaskDto newTask)
    {
        var task = await taskRepository.Read(id);
        if (task is null)
        {
            return Result<TaskDto>.Failure(TaskError.TaskNotFound);
        }
        
        task.Update(newTask.Name, newTask.Notes, newTask.Status, newTask.PlannedStartHour, newTask.PlannedEndHour);
        taskRepository.Update(task);
        await unitOfWork.SaveChangesAsync();
        
        var taskDto = mapper.Map<TaskDto>(task);
        
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<bool> Delete(TaskId id)
    {
        var task = await taskRepository.Read(id);
        if (task is null)
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