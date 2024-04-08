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
    public string Name { get; set; }
    public string Notes { get; set; }
    public TaskStatus Status { get; set; }
    public User Creator { get; set; }
    public Guid? GroupId { get; set; }
    public DateTime PlannedAt { get; set; }
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
    
}