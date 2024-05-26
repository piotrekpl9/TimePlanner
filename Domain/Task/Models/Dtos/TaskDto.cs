using Domain.User.Models.Dtos;

namespace Domain.Task.Models.Dtos;
using Domain.User.Entities;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;


public class TaskDto
{
    public TaskDto(
        string name, 
        string notes, 
        string status, 
        UserDto creator, 
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
    public string Status { get; private set; }
    public UserDto Creator { get; private set; }
    public Guid? GroupId { get; private set; }
    public DateTime PlannedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
}