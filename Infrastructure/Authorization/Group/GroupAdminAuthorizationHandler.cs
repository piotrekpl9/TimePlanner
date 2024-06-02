using System.Security.Claims;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Group;

using Task = System.Threading.Tasks.Task;
public class GroupAdminAuthorizationHandler(IGroupRepository groupRepository)
    : AuthorizationHandler<IsGroupAdminRequirement, GroupId>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsGroupAdminRequirement requirement, GroupId groupId)
    {
        
        var userId = new UserId(Guid.Parse( context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var group = await groupRepository.Read(groupId);
        if (group is null)
        {
            context.Fail();
            return;
        }

        var member = group.Members.FirstOrDefault(member => member.User.Id.Equals(userId));
        if (member is not null)
        {
            if (member.Role == Role.Admin)
            {
                context.Succeed(requirement);
            }
        }
    }
}