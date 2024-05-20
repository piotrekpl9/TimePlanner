using Domain.Group.Models.Dtos;
using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.User.ValueObjects;

namespace Domain.Group.Services;
using Entities;

public interface IGroupService
{
    public Task<Result<Group>> CreateGroup(String name, UserId userId);
    //Only allow members to read content of the group
    public Task<Result<Group>> ReadGroup(GroupId id, UserId userId);   
    public Task<Result<InvitationDto>> InviteUserByEmail(string email, GroupId groupId, UserId invitingUserId);   
    public Task<Result> CancelInvitation(InvitationId id, UserId userId);
    public Task<Result> AcceptInvitation(InvitationId id, UserId userId);
    public Task<Result> RejectInvitation(InvitationId id, UserId userId);

    public Task<Result<Group>> DeleteGroup(GroupId id, UserId userId);
    public Task<Result> DeleteMember(MemberId id, UserId userId);
}