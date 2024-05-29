namespace Domain.Task.Models;

public class CreateTaskRequest
{
    public CreateTaskRequest(string name, string notes, DateTime plannedStartHour, DateTime plannedEndHour)
    {
        Name = name;
        Notes = notes;
        PlannedStartHour = plannedStartHour;
        PlannedEndHour = plannedEndHour;
    }
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public DateTime PlannedStartHour { get; private set; }
    public DateTime PlannedEndHour { get; private set; }
    
}