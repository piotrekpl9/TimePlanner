using Domain.Group.Models.ValueObjects;
using Domain.Shared;
using Domain.Task.Models.ValueObjects;
using Domain.Task.TaskErrors;

namespace Domain.Task.Entities;
using Domain.User.Entities;
using Primitives;
using Models.Enums;
public sealed class Task : Entity<TaskId>
{
    private readonly List<User> _assignedUsers;
    public IReadOnlyCollection<User> AssignedUsers => _assignedUsers.ToList();
    public string Name { get; set; }
    public string Notes { get; set; }
    public TaskStatus Status { get; set; }
    public User Creator { get; set; }
    public GroupId? GroupId { get; set; }
    public DateTime PlannedStartHour { get; set; }
    public DateTime PlannedEndHour { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    private Task(
        TaskId id, 
        string name, 
        string notes, 
        TaskStatus status, 
        List<User> assignedUsers, 
        User creator, 
        GroupId? groupId, 
        DateTime plannedStartHour, 
        DateTime plannedEndHour, 
        DateTime createdAt, 
        DateTime? updatedAt, 
        DateTime? deletedAt
        ) : base(id)
    {
        Name = name;
        Notes = notes;
        Status = status;
        _assignedUsers = assignedUsers;
        Creator = creator;
        GroupId = groupId;
        PlannedStartHour = plannedStartHour;
        PlannedEndHour = plannedEndHour;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    private Task(TaskId id) : base(id) {}
    public Task(TaskId id, string name, User creator, GroupId? groupId): base(id)
    {
        Name = name;
        Creator = creator;
        GroupId = groupId;
        _assignedUsers = new List<User>();
        CreatedAt = DateTime.UtcNow;
    }
    private Task(
        TaskId id, 
        string name, 
        string notes, 
        TaskStatus status, 
        DateTime plannedStartHour, 
        DateTime plannedEndHour, 
        DateTime createdAt, 
        DateTime? updatedAt, 
        DateTime? deletedAt
    ) : base(id)
    {
        Name = name;
        Notes = notes;
        Status = status;
        PlannedStartHour = plannedStartHour;
        PlannedEndHour = plannedEndHour;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
    
    public static Task CreateForSelf(string name, string notes, TaskStatus status, User user, DateTime plannedStartHour, DateTime plannedEndHour)
    {
        if (plannedStartHour > plannedEndHour)
        {
            plannedEndHour = plannedStartHour.AddMinutes(1);
        }
        return new Task(TaskId.Create(), name, notes, status, [user], user, null, plannedStartHour, plannedEndHour, DateTime.UtcNow, null, null);
    }
    
    public static Task CreateForGroup(string name, string notes, TaskStatus status, List<User> users, GroupId groupId, User creator, DateTime plannedStartHour, DateTime plannedEndHour)
    {
        return new Task(TaskId.Create(), name, notes, status, users, creator, groupId,  plannedStartHour, plannedEndHour, DateTime.UtcNow, null, null);
    }

    public void Update(string? name, string? notes, TaskStatus? status, DateTime? plannedStartHour, DateTime? plannedEndHour)
    {
        Name = name ?? Name;
        Notes = notes ?? Notes;
        Status = status ?? Status;
        PlannedStartHour = plannedStartHour ?? PlannedStartHour;
        
        if (PlannedStartHour > PlannedEndHour || PlannedStartHour > plannedEndHour)
        {
            plannedEndHour = PlannedStartHour;
        }
        PlannedEndHour = plannedEndHour ?? PlannedEndHour;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public Result<Task> Delete()
    {
        if (DeletedAt is not null)
        {
            return Result<Task>.Failure(TaskError.TaskAlreadyDeleted);
        }

        DeletedAt = DateTime.UtcNow;
        return Result<Task>.Success(this);
    }

    public Result AssignUser(User user)
    {
        _assignedUsers.Add(user);
        return Result.Success();
    }
}