using System.Net.Mime;
using System.Security.Claims;
using Application.Authentication.Model;
using AutoMapper;
using Domain.Group.Entities;
using Domain.Group.Models;
using Domain.Group.Models.Dtos;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.Group.Services;
using Domain.Shared;
using Domain.Task.Models.Dtos;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Model;
using Presentation.Model.Requests;
using Group = Domain.Group.Entities.Group;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController(IGroupService groupService, IGroupRepository groupRepository, IAuthorizationService authorizationService, IMapper mapper) : Controller
{
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> CreateGroup(
        [FromBody] CreateGroupRequest createRequest, 
        [FromServices] IValidator<CreateGroupRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(createRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
     
        var result = await groupService.CreateGroup(createRequest.Name, userId);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
      
            
    }
    
    [HttpPost("{groupGuid:guid}/invitations/create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> InviteUser(
        Guid groupGuid, 
        [FromBody] InviteUserRequest inviteUserRequest, 
        [FromServices] IValidator<InviteUserRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(inviteUserRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ReadGroup()
    {
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var result = await groupRepository.ReadGroupByUserId(userId);
        return result is not null ? Results.Ok(mapper.Map<GroupDto>(result)) : Results.NotFound();
    }

    
    [HttpGet("{groupGuid:guid}/")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ReadGroup(Guid groupGuid)
    {
        var groupId = new GroupId(groupGuid);
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, groupId,"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        var result = await groupRepository.Read(groupId);
        return result is not null ? Results.Ok(mapper.Map<Group, GroupDto>(result)) : Results.NotFound();
    }
    
    [HttpGet("pending-invitations")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<InvitationDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ReadUsersPendingInvitations()
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var result = (await groupRepository.ReadInvitationsByUserId(userId))?.Where(invitation => invitation.Status is InvitationStatus.Pending).ToList();
        if (result is null)
        {
            return Results.NotFound();
        }
        List<InvitationDto> dtos = result.Select(mapper.Map<InvitationDto>).ToList();
        return Results.Ok(dtos);
    }
    
    [HttpGet("{groupGuid:guid}/invitations")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<InvitationDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ReadGroupInvitations(Guid groupGuid)
    {
        var groupId = new GroupId(groupGuid);
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, groupId,"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded )
        {
            return Results.Forbid();
        }
        var result = await groupRepository.Read(groupId);
        if (result is null)
        {
            return Results.NotFound();
        }
        List<InvitationDto> dtos = result.Invitations.Select(mapper.Map<InvitationDto>).ToList();
        return Results.Ok(dtos);
    }
    
    [HttpGet("{groupGuid:guid}/members")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<MemberDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ReadGroupMembers(Guid groupGuid)
    { 
        var groupId = new GroupId(groupGuid);
        var memberAuthorizationResult = await authorizationService
            .AuthorizeAsync(User, groupId,"GroupAccessPolicy");
        
        if (!memberAuthorizationResult.Succeeded)
        {
            return Results.Forbid();
        }
        
        var result = await groupRepository.Read(groupId);
        
        if (result is null)
        {
            return Results.NotFound();
        }
        List<MemberDto> dtos = result.Members.Select(mapper.Map<MemberDto>).ToList();
        return Results.Ok(dtos);
    }
    
    [HttpDelete("{groupGuid:guid}/members/{memberId:guid}")]
    public async Task<IResult> DeleteGroupMember(Guid groupGuid,Guid memberId)
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
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Result<GroupDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> DeleteGroup(Guid groupGuid)
    { 
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
      
        var result = await groupService.DeleteGroup(new GroupId(groupGuid), userId);
        return result.IsSuccess ? Results.Ok(mapper.Map<GroupDto>(result.Value)) : Results.BadRequest(result.Error);
    }
}