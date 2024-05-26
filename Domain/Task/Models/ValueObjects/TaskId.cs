using Domain.Primitives;

namespace Domain.Task.Models.ValueObjects;

public class TaskId : ValueObject
{

    public TaskId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; }

    public static TaskId Create()
    {
        return new TaskId(Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}