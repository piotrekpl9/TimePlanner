using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.ValueObjects;

namespace Domain.Task.Models.Dtos;

public class UpdateTaskDto
{
    public UpdateTaskDto(TaskId id,string name, string notes, DateTime plannedAt, GroupId groupId,TaskStatus status)
    {
        Id = id;
        Name = name;
        Notes = notes;
        PlannedAt = plannedAt;
        GroupId = groupId;
        Status = status;
    }
    public TaskId Id { get; private set; }
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public GroupId GroupId { get; private set; }  
    public TaskStatus Status { get; private set; }
    public DateTime PlannedAt { get; private set; }
}