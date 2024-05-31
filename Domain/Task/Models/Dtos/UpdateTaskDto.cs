namespace Domain.Task.Models.Dtos;
using TaskStatus = Enums.TaskStatus;

public class UpdateTaskDto(
    string? name,
    string? notes,
    DateTime? plannedStartHour,
    DateTime? plannedEndHour,
    TaskStatus? status)
{
    public string? Name { get; private set; } = name;
    public string? Notes { get; private set; } = notes;
    public TaskStatus? Status { get; private set; } = status;
    public DateTime? PlannedStartHour { get; private set; } = plannedStartHour;
    public DateTime? PlannedEndHour { get; private set; } = plannedEndHour;
}