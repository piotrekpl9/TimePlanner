namespace Domain.Group.Models.Dtos;

public class CreateGroupDto(string name)
{
    public string Name { get; set; } = name;
}