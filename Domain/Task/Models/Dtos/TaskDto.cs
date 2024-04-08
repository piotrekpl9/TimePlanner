namespace Domain.Task.Models.Dtos;
using Domain.User.Entities;

public class TaskDto
{
    private TaskDto(
        string name, 
        string notes, 
        TaskStatus status, 
        User creator, 
        Guid? groupId, 
        DateTime plannedAt, 
        DateTime createdAt
    ) 
    {
        Name = name;
        Notes = notes;
        Status = status;
        Creator = creator;
        GroupId = groupId;
        PlannedAt = plannedAt;
        CreatedAt = createdAt;
    }
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public TaskStatus Status { get; private set; }
    public User Creator { get; private set; }
    public Guid? GroupId { get; private set; }
    public DateTime PlannedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
}