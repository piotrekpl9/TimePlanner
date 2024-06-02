using System.Security.Claims;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Group;

using Task = System.Threading.Tasks.Task;
public class GroupMemberAuthorizationHandler(IGroupRepository groupRepository)
    : AuthorizationHandler<IsGroupMemberRequirement, GroupId>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsGroupMemberRequirement requirement, GroupId groupId)
    {
        
        var userId = new UserId(Guid.Parse( context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var group = await groupRepository.Read(groupId);
        if (group is null)
        {
            context.Fail();
            return;
        }
        if (group.Members.Any(member => member.User.Id.Equals(userId)))
        {
            context.Succeed(requirement);
        }
    }
}