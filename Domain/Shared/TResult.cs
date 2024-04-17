namespace Domain.Shared;
public class Result<TValue> : Result
{
    protected internal Result(TValue? value, Error error) : base(error: error)
    {
        Value = value;
    }
    
    public readonly TValue? Value;
    
    public static Result<TValue> Success(TValue? value) => new(value, Error.None);

    public static Result<TValue> Failure(Error error) => new(default,error);

}
