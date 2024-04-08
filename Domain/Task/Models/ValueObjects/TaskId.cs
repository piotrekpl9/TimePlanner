using Domain.Primitives;

namespace Domain.Task.Models.ValueObjects;

public class TaskId : ValueObject
{

    private TaskId(Guid value)
    {
        Value = value;
    }
    private Guid Value { get; }

    public static TaskId Create()
    {
        return new TaskId(new Guid());
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}