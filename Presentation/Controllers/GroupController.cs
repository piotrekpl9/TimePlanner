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
public class GroupController(IGroupService groupService, IAuthorizationService authorizationService) : Controller
{
    
    [HttpPost("create")]
    public async Task<IResult> CreateGroup([FromBody] CreateGroupRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
     
        var result = await groupService.CreateGroup(createRequest.Name, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
    
    [HttpPost("{groupGuid:guid}/invitations/create")]
    public async Task<IResult> InviteUser(Guid groupGuid, [FromBody] InviteUserRequest inviteUserRequest)
    {
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var groupId = new GroupId(groupGuid);
        
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, groupId,"GroupAccessPolicy");

        if (!memberAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await groupService.InviteUserByEmail(inviteUserRequest.Email, groupId, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
     
    [HttpPost("{groupGuid:guid}/invitations/{id:guid}/accept")]
    public async Task<IResult> AcceptInvitation(Guid groupGuid, Guid id)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");

        var invitationTargetAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new InvitationId(id),"InvitationOperationPolicy");
        
        if (!memberAuthorizationResult.Succeeded || !invitationTargetAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await groupService.AcceptInvitation(new InvitationId(id));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    
    [HttpPost("{groupGuid:guid}/invitations/{id:guid}/reject")]
    public async Task<IResult> RejectInvitation(Guid groupGuid, Guid id)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");

        var invitationTargetAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new InvitationId(id),"InvitationOperationPolicy");
        
        if (!memberAuthorizationResult.Succeeded || !invitationTargetAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await groupService.RejectInvitation(new InvitationId(id));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpPost("{groupGuid:guid}/invitations/{id:guid}/cancel")]
    public async Task<IResult> CancelInvitation(Guid groupGuid, Guid id)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");
        var invitationCancelAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"InvitationCancelPolicy");

        if (!memberAuthorizationResult.Succeeded || !invitationCancelAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await groupService.CancelInvitation(new InvitationId(id));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpPost("{groupGuid:guid}/leave")]
    public async Task<IResult> LeaveGroup(Guid groupGuid)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.Leave(userId, new GroupId(groupGuid));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpGet("{groupGuid:guid}/")]
    public async Task<IResult> GetGroup(Guid groupGuid)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        
        var result = await groupService.ReadGroup(new GroupId(groupGuid));
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpGet("{groupGuid:guid}/invitations")]
    public async Task<IResult> GetGroupInvitations(Guid groupGuid)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        
        var result = await groupService.ReadGroup(new GroupId(groupGuid));
        return result.IsSuccess ? Results.Ok(result.Value?.Invitations ?? []) : Results.BadRequest(result.Error);
    }
    
    [HttpGet("{groupGuid:guid}/members")]
    public async Task<IResult> GetGroupMembers(Guid groupGuid)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        
        var result = await groupService.ReadGroup(new GroupId(groupGuid));
        return result.IsSuccess ? Results.Ok(result.Value?.Members ?? []) : Results.BadRequest(result.Error);
    }
    
    [HttpDelete("{groupGuid:guid}/members/{memberId:guid}")]
    public async Task<IResult> GetGroupMembers(Guid groupGuid,Guid memberId)
    { 
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, new GroupId(groupGuid),"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        var result = await groupService.DeleteMember(new GroupId(groupGuid),new MemberId(memberId));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpDelete("{groupGuid:guid}/delete")]
    public async Task<IResult> DeleteGroup(Guid groupGuid)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.DeleteGroup(new GroupId(groupGuid), userId);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
}