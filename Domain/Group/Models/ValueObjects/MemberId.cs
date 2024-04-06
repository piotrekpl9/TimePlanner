using Domain.Primitives;

namespace Domain.Group.Models.ValueObjects;

public class MemberId : ValueObject
{
    public Guid Value { get; }

    public MemberId(Guid value)
    {
        Value = value;
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}