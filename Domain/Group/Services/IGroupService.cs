using Domain.Group.Models.Dtos;
using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Group.Services;
using Entities;

public interface IGroupService
{
    public Task<Group> CreateGroup(String name, Guid userId);
    //Only allow members to read content of the group
    public Task<Group> ReadGroup(GroupId id);   
    public Task<Group> ReadGroupByMemberUserId(UserId id);   

    public Task<InvitationDto> InviteUserByEmail(string email);   
    public Task<InvitationDto> CancelInvitation(InvitationId id);

    public Task<Group> DeleteGroup(GroupId id);
    public Task<MemberDto> DeleteMember(MemberId id);
}