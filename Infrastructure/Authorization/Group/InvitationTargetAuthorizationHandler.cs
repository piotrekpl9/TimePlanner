using System.Security.Claims;
using Domain.Group.Models.ValueObjects;
using Domain.Group.Repositories;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Group;

using Task = System.Threading.Tasks.Task;

public class InvitationTargetAuthorizationHandler(IGroupRepository groupRepository)
    : AuthorizationHandler<IsInvitationTargetRequirement, InvitationId>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsInvitationTargetRequirement requirement, InvitationId invitationId)
    {
        
        var userId = new UserId(Guid.Parse( context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var group = await groupRepository.ReadGroupByInvitationId(invitationId);
        if (group != null && group.Invitations.Any(invitation => invitation.User.Id.Equals(userId)))
        {
            context.Succeed(requirement);
        }
    }
}