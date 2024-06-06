using Application.Common.Data;
using AutoMapper;
using Domain.Group.Errors;
using Domain.Group.Repositories;
using Domain.Shared;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Repositories;
using Domain.Task.Services;
using Domain.Task.TaskErrors;
using Domain.User.Errors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Microsoft.Extensions.Logging;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;

namespace Application.Task;
using Task = Domain.Task.Entities.Task;

public class TaskService(ITaskRepository taskRepository, IGroupRepository groupRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<TaskService> logger) : ITaskService
{
    public async Task<Result<TaskDto>> CreateForGroup(CreateTaskDto createDto, UserId userId)
    {
        logger.Log(LogLevel.Information, "Create task for group");
        var creator = await userRepository.GetById(userId);
        if (creator is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} {userId.Value}" );
            return Result<TaskDto>.Failure(UserError.DoesntExists);
        }

        var group = await groupRepository.ReadGroupByUserId(userId);
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was userId: {userId.Value}" );
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
        logger.Log(LogLevel.Information, "Create task for user");
        var creator = await userRepository.GetById(userId);
        if (creator is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} {userId.Value}" );
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
        logger.Log(LogLevel.Information, "Update task");
        var task = await taskRepository.Read(id);
        if (task is null)
        {        
            logger.Log(LogLevel.Error, $"{TaskError.TaskNotFound.Description} Search criteria was taskId: {id}" );
            return Result<TaskDto>.Failure(TaskError.TaskNotFound);
        }
        
        task.Update(newTask.Name, newTask.Notes, newTask.Status, newTask.PlannedStartHour, newTask.PlannedEndHour);
        taskRepository.Update(task);
        await unitOfWork.SaveChangesAsync();
        
        var taskDto = mapper.Map<TaskDto>(task);
        
        return Result<TaskDto>.Success(taskDto);
    }

    public async Task<Result> Delete(TaskId id)
    {
        logger.Log(LogLevel.Information, "Delete task");
        var task = await taskRepository.Read(id);
        if (task is null)
        {
            logger.Log(LogLevel.Error, $"{TaskError.TaskNotFound.Description} Search criteria was taskId: {id}" );
            return Result.Failure(TaskError.TaskNotFound);
        }

        var result = task.Delete();
        if(result.IsFailure)
        {   
            logger.Log(LogLevel.Error, $"{result.Error.Description} taskId: {id}" );
            return result;
        }
        
        taskRepository.Update(task);
        await unitOfWork.SaveChangesAsync();
        return result;
    }
}