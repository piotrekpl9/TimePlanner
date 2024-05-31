using Application.Common.Data;
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

namespace Application.Group;
using Domain.Group.Entities;

public class GroupService( 
    IGroupRepository groupRepository,
    IUserRepository userRepository,
    ITaskRepository taskRepository,
    IUnitOfWork unitOfWork, 
    IMapper mapper) : IGroupService
{
    public async Task<Result<GroupDto>> CreateGroup(string name, UserId userId)
    {
        var user = await userRepository.GetById(userId);
        if (user is null)
        {
            return Result<GroupDto>.Failure(UserError.DoesntExists);
        }
        
        var exitingGroupCheck = await groupRepository.ReadGroupByUserId(userId);

        if (exitingGroupCheck is not null)
        {
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
        var user = await userRepository.GetById(invitingUserId);
        var targetUser = await userRepository.GetByEmail(email);
        if (user is null || targetUser is null)
        {
            return Result<InvitationDto>.Failure(UserError.DoesntExists);
        }

        if (targetUser.Email == user.Email)
        {
            return Result<InvitationDto>.Failure(GroupError.UserInvitingItself);
        }
        
        var groupResult = await ReadGroup(groupId);
        if (groupResult.IsFailure)
        {
            return Result<InvitationDto>.Failure(groupResult.Error);
        }

        if (groupResult.Value is null)
        {
            return Result<InvitationDto>.Failure(Error.NullSuccessValue);
        }
        var group = groupResult.Value;
        
        var invitationResult = group.Invite(targetUser, invitingUserId);
        if (invitationResult.IsFailure)
        {
            return Result<InvitationDto>.Failure(invitationResult.Error);
        }
        if (invitationResult.Value is null)
        {
            return Result<InvitationDto>.Failure(Error.NullSuccessValue);
        }
        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();

        var invitationDto =  mapper.Map<InvitationDto>(invitationResult.Value);
        
        return Result<InvitationDto>.Success(invitationDto);
    }

    public async Task<Result> CancelInvitation(InvitationId id)
    {
        var group = await groupRepository.ReadGroupByInvitationId(id);
        
        if (group is null)
        {
            return Result<InvitationDto>.Failure(GroupError.GroupNotFound);
        }

        var invitation = group.Invitations.FirstOrDefault(i => i.Id.Equals(id));
        if (invitation is null)
        {
            return Result<InvitationDto>.Failure(GroupError.InvitationNotFound); 
        }
        
        var result = group.CancelInvitation(invitation);
        
        if (result.IsFailure)
        {
            return result;
        }
        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> AcceptInvitation(InvitationId id)
    {
        var group = await groupRepository.ReadGroupByInvitationId(id);
        if (group is null)
        {
            return Result<InvitationDto>.Failure(GroupError.GroupNotFound);
        }

        var invitation = group.Invitations.FirstOrDefault(i => i.Id.Equals(id));
        if (invitation is null)
        {
            return Result<InvitationDto>.Failure(GroupError.InvitationNotFound); 
        }

        var user = await userRepository.GetById(invitation.User.Id);
        if (user is null)
        {
            return Result.Failure(UserError.DoesntExists);
        }
        
        var result = group.AcceptInvitation(invitation);
      
        if (result.IsFailure)
        {
            return result;
        }

        if (result.Value is null)
        {
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
        var group = await groupRepository.ReadGroupByInvitationId(id);
        
        if (group is null)
        {
            return Result<InvitationDto>.Failure(GroupError.GroupNotFound);
        }

        var invitation = group.Invitations.FirstOrDefault(i => i.Id.Equals(id));
        if (invitation is null)
        {
            return Result<InvitationDto>.Failure(GroupError.InvitationNotFound); 
        }
        
        var result = group.RejectInvitation(invitation);
        if (result.IsFailure)
        {
            return result;
        }
        
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<GroupDto>> DeleteGroup(GroupId id, UserId userId)
    {
        var group = await groupRepository.Read(id);
        if (group is null)
        {
            return Result<GroupDto>.Failure(GroupError.GroupNotFound);
        }

        var user = group.Members.FirstOrDefault(member => member.User.Id.Equals(userId));
        if (user is not null && user.Role is not Role.Admin)
        {
            return Result<GroupDto>.Failure(GroupError.UserIsNotGroupOwner);
        }
        
        var res = group.Delete();

        if (res.IsFailure)
        {
            return Result<GroupDto>.Failure(res.Error);
        }

        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync();
        
        return Result<GroupDto>.Success(mapper.Map<GroupDto>(group));
    }

    public async Task<Result> DeleteMember(GroupId groupId, MemberId id)
    {
        var group = await groupRepository.Read(groupId);
        if (group is null)
        {
            return Result.Failure(GroupError.GroupNotFound);
        }
        
        var result = group.RemoveMember(id);

        return result;
    }

    public async Task<Result> Leave(UserId userId, GroupId groupId)
    {
        var group = await groupRepository.Read(groupId);
        if (group is null)
        {
            return Result.Failure(GroupError.GroupNotFound);
        }
        
        var member = group.Members.FirstOrDefault(member => member.User.Id.Equals(userId));
        if (member is null)
        {
            return Result.Failure(GroupError.UserIsNotMember);
        }

        return group.RemoveMember(member.Id);
    }
}