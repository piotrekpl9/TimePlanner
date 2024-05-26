using System.Security.Claims;
using Domain.Group.Models;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Services;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController(IGroupService groupService) : Controller
{
    [HttpPost("create")]
    [Authorize]
    public async Task<IResult> CreateGroup([FromBody] CreateGroupRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await groupService.CreateGroup(createRequest.Name, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
    
    [HttpPost("invitations/create")]
    [Authorize]
    public async Task<IResult> InviteUser([FromBody] InviteUserRequest inviteUserRequest)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await groupService.InviteUserByEmail(inviteUserRequest.Email, new GroupId(inviteUserRequest.GroupId), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
     
    [HttpPost("invitations/{id:guid}/accept")]
    [Authorize]
    public async Task<IResult> AcceptInvitation(Guid id)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.AcceptInvitation(new InvitationId(id), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    
    [HttpPost("invitations/{id:guid}/reject")]
    [Authorize]
    public async Task<IResult> RejectInvitation(Guid id)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.RejectInvitation(new InvitationId(id), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpPost("invitations/{id:guid}/cancel")]
    [Authorize]
    public async Task<IResult> CancelInvitation(Guid id)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.CancelInvitation(new InvitationId(id), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpPost("leave")]
    [Authorize]
    public async Task<IResult> LeaveGroup([FromBody] LeaveGroupRequest leaveGroupRequest)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.Leave(userId, new GroupId(leaveGroupRequest.GroupGuid));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpDelete("{id:guid}/delete")]
    [Authorize]
    public async Task<IResult> DeleteGroup(Guid id)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.DeleteGroup(new GroupId(id), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
}