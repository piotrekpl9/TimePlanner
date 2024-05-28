using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.ValueObjects;

namespace Domain.Task.Models;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;

public class UpdateTaskRequest
{
    public UpdateTaskRequest(string? name, string? notes, DateTime? plannedAt, TaskStatus? status)
    {
        Name = name;
        Notes = notes;
        PlannedAt = plannedAt;
        Status = status;
    }
    
    public string? Name { get; private set; }
    public string? Notes { get; private set; }
    public TaskStatus? Status { get; private set; }
    public DateTime? PlannedAt { get; private set; }
}