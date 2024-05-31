namespace Domain.Task.Models.Dtos;

public class CreateTaskDto(string name, string notes, DateTime plannedStartHour, DateTime plannedEndHour)
{
    public string Name { get; private set; } = name;
    public string Notes { get; private set; } = notes;
    public DateTime PlannedStartHour { get; private set; } = plannedStartHour;
    public DateTime PlannedEndHour { get; private set; } = plannedEndHour;
}