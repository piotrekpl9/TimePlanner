using System.Collections.ObjectModel;
using Domain.Task.Models.ValueObjects;

namespace Domain.Task.Entities;
using Domain.User.Entities;
using Primitives;
using Models.Enums;
public sealed class Task : Entity<TaskId>
{
    private readonly List<User> _assignedUsers;
    public IReadOnlyCollection<User> AssignedUsers => _assignedUsers.AsReadOnly();
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public TaskStatus Status { get; private set; }
    public User Creator { get; private set; }
    public Guid? GroupId { get; private set; }
    public DateTime PlannedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    
    private Task(
        TaskId id, 
        string name, 
        string notes, 
        TaskStatus status, 
        List<User> assignedUsers, 
        User creator, 
        Guid? groupId, 
        DateTime plannedAt, 
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
        PlannedAt = plannedAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public static Task Create(string name, string notes, List<User> usersToAssign, User creator, Guid? groupId, DateTime plannedAt)
    {
        return new Task(TaskId.Create(), name, notes, TaskStatus.Pending, usersToAssign, creator, groupId, plannedAt, DateTime.UtcNow, null, null);
    }
    
}