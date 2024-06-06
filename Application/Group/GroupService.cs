using Application.Common.Data;
using Application.Task;
using AutoMapper;
using Domain.Group.Errors;
using Domain.Group.Models.Dtos;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.Group.Services;
using Domain.Shared;
using Domain.Task.Repositories;
using Domain.User.Errors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Application.Group;
using Domain.Group.Entities;

public class GroupService( 
    IGroupRepository groupRepository,
    IUserRepository userRepository,
    ITaskRepository taskRepository,
    IUnitOfWork unitOfWork,
    ILogger<GroupService> logger,
    IMapper mapper) : IGroupService
{
    public async Task<Result<GroupDto>> CreateGroup(string name, UserId userId)
    {
        logger.Log(LogLevel.Information, "Create group");
        var user = await userRepository.GetById(userId);
        if (user is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} Search criteria was taskId: {userId}");
            return Result<GroupDto>.Failure(UserError.DoesntExists);
        }
        
        var exitingGroupCheck = await groupRepository.ReadGroupByUserId(userId);

        if (exitingGroupCheck is not null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.UserAlreadyInOtherGroup.Description} {userId}");
            return Result<GroupDto>.Failure(GroupError.UserAlreadyInOtherGroup);
        }
        
        var group = Group.Create(name, user);
        await groupRepository.Add(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result<GroupDto>.Success(mapper.Map<GroupDto>(group));
    }

    private async Task<Result<Group>> ReadGroup(GroupId id)
    {
        var group = await groupRepository.Read(id);
        return group is null ? Result<Group>.Failure(GroupError.GroupNotFound) : Result<Group>.Success(group);
    }

    public async Task<Result<InvitationDto>> InviteUserByEmail(string email, GroupId groupId,  UserId invitingUserId)
    {
        logger.Log(LogLevel.Information, "Invite user by email");

        var user = await userRepository.GetById(invitingUserId);
        var targetUser = await userRepository.GetByEmail(email);
        if (user is null || targetUser is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} Search criteria was userId: {invitingUserId} and email: {email}");
            return Result<InvitationDto>.Failure(UserError.DoesntExists);
        }

        if (targetUser.Email == user.Email)
        {
            logger.Log(LogLevel.Error, $"{GroupError.UserInvitingItself.Description} requested email: {email}");
            return Result<InvitationDto>.Failure(GroupError.UserInvitingItself);
        }
        
        var groupResult = await ReadGroup(groupId);
        if (groupResult.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{groupResult.Error.Description} Search criteria was groupId: {groupId}");
            return Result<InvitationDto>.Failure(groupResult.Error);
        }

        if (groupResult.Value is null)
        {
            logger.Log(LogLevel.Error, $"{Error.NullSuccessValue.Description} for group");
            return Result<InvitationDto>.Failure(Error.NullSuccessValue);
        }
        var group = groupResult.Value;
        
        var invitationResult = group.Invite(targetUser, invitingUserId);
        if (invitationResult.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{invitationResult.Error.Description} Operation: Invite parameters: {targetUser}, {invitingUserId}");
            return Result<InvitationDto>.Failure(invitationResult.Error);
        }
        if (invitationResult.Value is null)
        {
            logger.Log(LogLevel.Error, $"{Error.NullSuccessValue.Description} for invitation");
            return Result<InvitationDto>.Failure(Error.NullSuccessValue);
        }
        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();

        var invitationDto =  mapper.Map<InvitationDto>(invitationResult.Value);
        
        return Result<InvitationDto>.Success(invitationDto);
    }

    public async Task<Result> CancelInvitation(InvitationId id)
    {
        logger.Log(LogLevel.Information, "Cancel invitation");

        var group = await groupRepository.ReadGroupByInvitationId(id);
        
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was invitationId: {id}");
            return Result<InvitationDto>.Failure(GroupError.GroupNotFound);
        }

        var invitation = group.Invitations.FirstOrDefault(i => i.Id.Equals(id));
        if (invitation is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.InvitationNotFound.Description} Search criteria was invitationId: {id}");
            return Result<InvitationDto>.Failure(GroupError.InvitationNotFound); 
        }
        
        var result = group.CancelInvitation(invitation);
        
        if (result.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{result.Error.Description} Operation: CancelInvitation");
            return result;
        }
        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> AcceptInvitation(InvitationId id)
    {
        logger.Log(LogLevel.Information, "Accept invitation");

        var group = await groupRepository.ReadGroupByInvitationId(id);
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was invitationId: {id}");
            return Result<InvitationDto>.Failure(GroupError.GroupNotFound);
        }

        var invitation = group.Invitations.FirstOrDefault(i => i.Id.Equals(id));
        if (invitation is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.InvitationNotFound.Description} Search criteria was invitationId: {id}");
            return Result<InvitationDto>.Failure(GroupError.InvitationNotFound); 
        }

        var user = await userRepository.GetById(invitation.User.Id);
        if (user is null)
        {
            logger.Log(LogLevel.Error, $"{UserError.DoesntExists.Description} Search criteria was userId: {invitation.User.Id.Value}");
            return Result.Failure(UserError.DoesntExists);
        }
        
        var result = group.AcceptInvitation(invitation);
      
        if (result.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{result.Error.Description} Operation: AcceptInvitation");
            return result;
        }

        if (result.Value is null)
        {
            logger.Log(LogLevel.Error, $"{Error.NullSuccessValue.Description} for AcceptInvitation");
            return Result.Failure(Error.NullSuccessValue);
        }
        
        groupRepository.Update(group);
        
        var tasks = await taskRepository.ReadAllByGroupId(group.Id);
      
        foreach (var task in tasks)
        {
            task.AssignUser(user);
            taskRepository.Update(task);
        }
        
        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> RejectInvitation(InvitationId id)
    {
        logger.Log(LogLevel.Information, "Reject invitation");

        var group = await groupRepository.ReadGroupByInvitationId(id);
        
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was invitationId: {id}");
            return Result<InvitationDto>.Failure(GroupError.GroupNotFound);
        }

        var invitation = group.Invitations.FirstOrDefault(i => i.Id.Equals(id));
        if (invitation is null)
        {
            
            logger.Log(LogLevel.Error, $"{GroupError.InvitationNotFound.Description} Search criteria was invitationId: {id}");
            return Result<InvitationDto>.Failure(GroupError.InvitationNotFound); 
        }
        
        var result = group.RejectInvitation(invitation);
        if (result.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{result.Error.Description}  Operation: RejectInvitation");
            return result;
        }
        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<GroupDto>> DeleteGroup(GroupId id, UserId userId)
    {
        logger.Log(LogLevel.Information, "Delete group");

        var group = await groupRepository.Read(id);
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was groupId: {id}");
            return Result<GroupDto>.Failure(GroupError.GroupNotFound);
        }

        var user = group.Members.FirstOrDefault(member => member.User.Id.Equals(userId));
        if (user is not null && user.Role is not Role.Admin)
        {
            logger.Log(LogLevel.Error, $"{GroupError.UserIsNotGroupOwner.Description} userId: {userId}");
            return Result<GroupDto>.Failure(GroupError.UserIsNotGroupOwner);
        }
        
        var result = group.Delete();

        if (result.IsFailure)
        {
            logger.Log(LogLevel.Error, $"{result.Error.Description} Operation: Delete");
            return Result<GroupDto>.Failure(result.Error);
        }

        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result<GroupDto>.Success(mapper.Map<GroupDto>(group));
    }

    public async Task<Result> DeleteMember(GroupId groupId, MemberId id)
    {
        logger.Log(LogLevel.Information, "Delete member");

        var group = await groupRepository.Read(groupId);
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was groupId: {groupId}");
            return Result.Failure(GroupError.GroupNotFound);
        }
        
        var result = group.RemoveMember(id);
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task<Result> Leave(UserId userId, GroupId groupId)
    {
        logger.Log(LogLevel.Information, "Leave group");

        var group = await groupRepository.Read(groupId);
        if (group is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.GroupNotFound.Description} Search criteria was groupId: {groupId}");
            return Result.Failure(GroupError.GroupNotFound);
        }
        
        var member = group.Members.FirstOrDefault(member => member.User.Id.Equals(userId));
        if (member is null)
        {
            logger.Log(LogLevel.Error, $"{GroupError.UserIsNotMember.Description} userId: {userId}, groupId: {groupId}");
            return Result.Failure(GroupError.UserIsNotMember);
        }
        var result = group.RemoveMember(member.Id);

        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        return result;
    }
}