namespace Domain.Shared;

public class Error
{
    public string Name { get; }
    public string Code { get; }
    public string Description { get; }

    public Error(string name, string code, string description)
    {
        Code = code;
        Name = name;
        Description = description;
    }

    public static Error None => new Error(string.Empty, "COMMON_0",string.Empty);
    public static Error NullSuccessValue => new Error("NullSuccessValue","COMMON_1", "Result is success but returned value is null!");
    public static Error Unknown => new Error("Unknown","COMMON_2", "Unknown error occured!");

    
    public bool Equals(Error other)
    {
        return Name == other.Name && Description == other.Description;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Description);
    }
}