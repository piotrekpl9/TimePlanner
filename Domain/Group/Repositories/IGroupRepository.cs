using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Group.Repositories;
using Entities;
public interface IGroupRepository
{
    public System.Threading.Tasks.Task Add(Group group);
    
    public Task<Group?> Read(GroupId groupId);
    public Task<Group?> ReadGroupByInvitationId(InvitationId invitationId);
    public Task<Group?> ReadGroupByMemberId(MemberId memberId);
    public Task<Group?> ReadGroupByUserId(UserId userId);
    public Task<List<Invitation>?> ReadInvitationsByUserId(UserId userId);
    public void Update(Group group);
    public Task<bool> Delete(GroupId groupId);
}