namespace Domain.Task.Models;

public class CreateTaskRequest
{
    public CreateTaskRequest(string name, string notes, DateTime plannedAt)
    {
        Name = name;
        Notes = notes;
        PlannedAt = plannedAt;
    }
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public DateTime PlannedAt { get; private set; }
    
}