namespace Domain.Shared;
public class Result
{
    protected Result(Error error)
    {

        IsSuccess = error == Error.None;
        Error = error;
    }
    
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    
    public readonly Error Error;
    
    public static Result Success() => new(Error.None);

    public static Result Failure(Error error) => new(error);

}
