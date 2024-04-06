using Domain.Group.Models.ValueObjects;

namespace Domain.Group.Repositories;
using Entities;
public interface IGroupRepository
{
    public Task<Group> Create(Group group);
    
    public Task<Group> Read(GroupId groupId);
    //Here is a definition for saving all operations such as invite or create member
    public Task<Group> Update(Group group);
    public Task<Group> Delete(GroupId groupId);
}