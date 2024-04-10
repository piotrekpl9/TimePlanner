namespace Domain.Shared;

public class Error
{
    public string Code { get; }
    public string Description { get; }

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public static Error None => new Error(string.Empty, string.Empty);
    public static Error NullSuccessValue => new Error("NullSuccessValue", "Result is success but returned value is null!");
}