using System.Security.Claims;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Group;

using Task = System.Threading.Tasks.Task;

public class InvitationCreatorAuthorizationHandler(IGroupRepository groupRepository)
    : AuthorizationHandler<IsInvitationCreatorRequirement, InvitationId>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsInvitationCreatorRequirement requirement, InvitationId invitationId)
    {
        var userId = new UserId(Guid.Parse( context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var group = await groupRepository.ReadGroupByInvitationId(invitationId);
        if (group is null)
        {
            context.Fail();
            return;
        }
        var member = group.Members.FirstOrDefault(m => m.User.Id.Equals(userId));
        if (member is null)
        {
            context.Fail();
            return;
        }
        if (group.Invitations.Any(invitation => invitation.Creator.Id.Equals(member.Id)))
        {
            context.Succeed(requirement);
        }
    }
}