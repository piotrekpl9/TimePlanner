using Domain.Group.Models.Dtos;
using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.User.ValueObjects;

namespace Domain.Group.Services;
using Entities;

public interface IGroupService
{
    public Task<Result<Group>> CreateGroup(string name, UserId userId);
    public Task<Result<InvitationDto>> InviteUserByEmail(string email, GroupId groupId, UserId invitingUserId);   
    public Task<Result> CancelInvitation(InvitationId id);
    public Task<Result> AcceptInvitation(InvitationId id);
    public Task<Result> RejectInvitation(InvitationId id);

    public Task<Result<Group>> DeleteGroup(GroupId id, UserId userId);
    public Task<Result> DeleteMember(GroupId groupId, MemberId id);
    public Task<Result> Leave(UserId userId, GroupId group);
}