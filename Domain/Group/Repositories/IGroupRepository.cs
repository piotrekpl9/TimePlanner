using Domain.Group.Models.ValueObjects;
using Domain.User.ValueObjects;

namespace Domain.Group.Repositories;
using Entities;
public interface IGroupRepository
{
    public System.Threading.Tasks.Task Add(Group group);
    
    public Task<Group?> Get(GroupId groupId);
    public Task<Group?> GetGroupByInvitationId(InvitationId invitationId);
    public Task<Group?> GetGroupByMemberId(MemberId memberId);
    public Task<Group?> GetGroupByUserId(UserId userId);
    public Task<List<Invitation>?> GetInvitationsByUserId(UserId userId);
    public void Update(Group group);
    public Task<bool> Delete(GroupId groupId);
}