using System.Security.Claims;
using Application.Authentication.Model;
using Domain.Group.Models;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Services;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroupService groupService) : Controller
{
    [HttpPost]
    [Authorize]
    [Route("/create")]
    public async Task<IResult> CreateGroup([FromBody] CreateGroupRequest createRequest)
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await groupService.CreateGroup(createRequest.Name, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
    
    [HttpPost]
    [Authorize]
    [Route("/inviteUser")]
    public async Task<IResult> InviteUser([FromBody] InviteUserRequest inviteUserRequest)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));

        var result = await groupService.InviteUserByEmail(inviteUserRequest.Email, new GroupId(inviteUserRequest.GroupId), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
     
    [HttpPost]
    [Authorize]
    [Route("/acceptInvitation")]
    public async Task<IResult> AcceptInvitation([FromBody] AcceptInvitationRequest acceptInvitationRequest)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.AcceptInvitation(new InvitationId(acceptInvitationRequest.InvitationGuid), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    
    [HttpPost]
    [Authorize]
    [Route("/rejectInvitation")]
    public async Task<IResult> RejectInvitation([FromBody] RejectInvitationRequest rejectInvitationRequest)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.RejectInvitation(new InvitationId(rejectInvitationRequest.InvitationGuid), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
    
    [HttpPost]
    [Authorize]
    [Route("/cancelInvitation")]
    public async Task<IResult> RejectInvitation([FromBody] CancelInvitationRequest cancelInvitationRequest)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.CancelInvitation(new InvitationId(cancelInvitationRequest.InvitationGuid), userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }
}