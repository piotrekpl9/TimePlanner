namespace Domain.Shared;
public class Result<TValue, TError> 
{
    protected internal Result(TValue? value)
    {
        IsSuccess = true;
        Value = value;
    }

    protected internal Result(TError error)
    {

        IsSuccess = false;
        Error = error;
    }
    
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    
    public readonly TValue? Value;
    public readonly TError? Error;
    
    public static Result<TValue,TError> Success(TValue? value) => new(value);

    public static Result<TValue,TError> Failure(TError error) => new(error);

}
