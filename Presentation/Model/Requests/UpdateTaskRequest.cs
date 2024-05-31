namespace Presentation.Model.Requests;
using TaskStatus = Domain.Task.Models.Enums.TaskStatus;

public class UpdateTaskRequest
{
    public UpdateTaskRequest(string? name, string? notes, DateTime? plannedStartHour, DateTime? plannedEndHour, TaskStatus? status)
    {
        Name = name;
        Notes = notes;
        PlannedStartHour = plannedStartHour;
        PlannedEndHour = plannedEndHour;
        Status = status;
    }
    
    public string? Name { get; private set; }
    public string? Notes { get; private set; }
    public TaskStatus? Status { get; private set; }
    public DateTime? PlannedStartHour { get; private set; }
    public DateTime? PlannedEndHour { get; private set; }
}