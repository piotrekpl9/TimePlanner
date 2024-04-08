namespace Domain.Task.Models.Dtos;

public class CreateTaskDto
{
    public CreateTaskDto(string name, string notes, DateTime plannedAt)
    {
        Name = name;
        Notes = notes;
        PlannedAt = plannedAt;
    }
    public string Name { get; private set; }
    public string Notes { get; private set; }
    public DateTime PlannedAt { get; private set; }
    
}