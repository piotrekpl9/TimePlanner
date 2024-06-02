using Domain.User.Models.Dtos;

namespace Domain.Task.Models.Dtos;
using Domain.User.Entities;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;


public class TaskDto
{
    public TaskDto()
    {
        
    }
    public TaskDto(
        Guid taskId,
        string name, 
        string notes, 
        string status, 
        UserDto creator, 
        Guid? groupId, 
        DateTime plannedStartHour, 
        DateTime plannedEndHour, 
        DateTime createdAt
    )
    {
        TaskId = taskId;
        Name = name;
        Notes = notes;
        Status = status;
        Creator = creator;
        GroupId = groupId;
        PlannedStartHour = plannedStartHour;
        PlannedEndHour = plannedEndHour;
        CreatedAt = createdAt;
    }
    public Guid TaskId { get; private set; }
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public string Status { get; private set; }
    public UserDto Creator { get; private set; }
    public Guid? GroupId { get; private set; }
    public DateTime PlannedStartHour { get; private set; }
    public DateTime PlannedEndHour { get; private set; }
    public DateTime CreatedAt { get; private set; }
}