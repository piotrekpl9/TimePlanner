using System.Security.Claims;
using AutoMapper;
using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Repositories;
using Domain.Task.Services;
using Domain.User.Models.Dtos;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Model;
using Presentation.Model.Requests;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService taskService, ITaskRepository taskRepository, IAuthorizationService authorizationService, IMapper mapper) : Controller
{
    [HttpPost("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> CreateTask(
        [FromBody] CreateTaskRequest createRequest, 
        [FromServices] IValidator<CreateTaskRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(createRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Create(mapper.Map<CreateTaskDto>(createRequest), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpPost("group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> CreateTaskForGroup(
        [FromBody] CreateTaskRequest createRequest, 
        [FromServices] IValidator<CreateTaskRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(createRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.CreateForGroup(mapper.Map<CreateTaskDto>(createRequest), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetTask(Guid id)
    {
        var taskId = new TaskId(id);
        var taskAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, taskId,"TaskAssignedPolicy");

        if (!taskAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var task = await taskRepository.Get(taskId);
        if (task is null)
        {
            return Results.NotFound();
        }
        return Results.Ok(mapper.Map<TaskDto>(task));

    }
    
    [HttpGet("user/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    public async Task<IResult> GetUserTasks()
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var tasks = await taskRepository.GetAllByUserId(userId);
        List<TaskDto> taskDtos = tasks.Select(mapper.Map<TaskDto>).ToList();

        return Results.Ok(taskDtos);
    }
    
    [HttpGet("group/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetGroupTasks(Guid id)
    {   
        var groupId = new GroupId(id);
        
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, groupId,"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        var tasks = await taskRepository.GetAllByGroupId(groupId);
        List<TaskDto> taskDtos = tasks.Select(mapper.Map<TaskDto>).ToList();
        return Results.Ok(taskDtos);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> UpdateTask(
        Guid id, 
        [FromBody] UpdateTaskRequest updateTaskRequest, 
        [FromServices] IValidator<UpdateTaskRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(updateTaskRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var taskId = new TaskId(id);
        var taskAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, taskId,"TaskAssignedPolicy");

        if (!taskAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await taskService.Update(new TaskId(id), mapper.Map<UpdateTaskDto>(updateTaskRequest));
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpDelete("{id:guid}")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> DeleteTask(Guid id)
    {   
        var taskId = new TaskId(id);
        var taskAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, taskId,"TaskAssignedPolicy");

        if (!taskAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await taskService.Delete(new TaskId(id));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
}