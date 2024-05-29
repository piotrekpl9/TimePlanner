using System.Security.Claims;
using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Services;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService taskService, IAuthorizationService authorizationService) : Controller
{
    [HttpPost("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> CreateTask([FromBody] CreateTaskRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Create(createRequest, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
    
    [HttpPost("group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> CreateTaskForGroup([FromBody] CreateTaskRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.CreateForGroup(createRequest, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }

    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> ReadTask(Guid id)
    {
        var taskId = new TaskId(id);
        var taskAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, taskId,"TaskAssignedPolicy");

        if (!taskAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await taskService.Read(taskId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpGet("user/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> ReadUserTasks()
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var result = await taskService.ReadUserTasks(userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpGet("group/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> ReadGroupTasks(Guid id)
    {   
        var groupId = new GroupId(id);
        
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, groupId,"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
     
        var result = await taskService.ReadGroupTasks(groupId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest updateTaskRequest)
    {   
        var taskId = new TaskId(id);
        var taskAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, taskId,"TaskAssignedPolicy");

        if (!taskAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await taskService.Update(new TaskId(id), updateTaskRequest);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpDelete("{id:guid}")]
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
        return result ? Results.Ok() : Results.BadRequest();

    }
}