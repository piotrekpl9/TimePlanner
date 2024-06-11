using System.Security.Claims;
using Domain.Task.Models.ValueObjects;
using Domain.Task.Repositories;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Task;

using Task = System.Threading.Tasks.Task;
public class TaskAssignedAuthorizationHandler(ITaskRepository taskRepository)
    : AuthorizationHandler<IsAssignedToTaskAuthorizationRequirement, TaskId>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAssignedToTaskAuthorizationRequirement requirement, TaskId taskId)
    {
        var userId = new UserId(Guid.Parse( context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var task = await taskRepository.Get(taskId);
        if (task != null && task.AssignedUsers.Any(user => user.Id.Equals(userId)))
        {
            context.Succeed(requirement);
        }
    }
}