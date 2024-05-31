using Application.Common.Data;
using Domain.Group.Entities;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.User.ValueObjects;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GroupRepository(IApplicationDbContext dbContext, IUnitOfWork unitOfWork) : IGroupRepository
{  
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task Add(Group group)
    {        
        await dbContext.Groups.AddAsync(group);
    }

    public async Task<Group?> Read(GroupId groupId)
    {
        return await dbContext.Groups.Include(group => group.Members).Include(group => group.Invitations).SingleOrDefaultAsync(group => groupId.Equals(group.Id));
    }

    public async Task<Group?> ReadGroupByInvitationId(InvitationId invitationId)
    {
        return await dbContext.Groups.Include(group => group.Invitations).Include(group => group.Members).Where(group => group.Invitations.Any(invitation => invitation.Id.Equals(invitationId))).SingleOrDefaultAsync();
    }

    public Task<Group?> ReadGroupByMemberId(MemberId memberId)
    {
        return dbContext.Groups.Include(group => group.Members).ThenInclude(member => member.User).Where(group => group.Members.Any(member => member.Id.Equals(memberId))).SingleOrDefaultAsync();
    }

    public Task<Group?> ReadGroupByUserId(UserId userId)
    {
        return dbContext.Groups.Include(group => group.Members).ThenInclude(member => member.User).Where(group => group.Members.Any(member => member.User.Id.Equals(userId))).SingleOrDefaultAsync();
    }

    public async Task<List<Invitation>?> ReadInvitationsByUserId(UserId userId)
    {
        return dbContext.Groups.Include(group => group.Invitations).AsNoTracking().SelectMany(group => group.Invitations.Where(invitation => invitation.User.Id.Equals(userId)).ToList()).ToList();
    }

    public void Update(Group group)
    {
        dbContext.Groups.Update(group);
    }

    public async Task<bool> Delete(GroupId groupId)
    {
        var group = await Read(groupId);
        if (group is null)
        {
            return false;
        }
        dbContext.Groups.Remove(group);
        return true;
    }
}