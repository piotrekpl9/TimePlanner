using System.Security.Claims;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Serivces;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : Controller
{
    [HttpPost("user")]
    [Authorize]
    public async Task<IResult> CreateTask([FromBody] CreateTaskRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Create(createRequest, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
    
    [HttpPost("group")]
    [Authorize]
    public async Task<IResult> CreateTaskForGroup([FromBody] CreateTaskRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.CreateForGroup(createRequest, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }

    
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IResult> ReadTask(Guid id)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Read(new TaskId(id), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpGet("user/{id:guid}")]
    [Authorize]
    public async Task<IResult> ReadUserTasks(Guid id)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Read(new TaskId(id), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpGet("group/{id:guid}")]
    [Authorize]
    public async Task<IResult> ReadGroupTasks(Guid id)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Read(new TaskId(id), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest updateTaskRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Update(new TaskId(id), updateTaskRequest, userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IResult> DeleteTask(Guid id)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await taskService.Read(new TaskId(id), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

    }
}