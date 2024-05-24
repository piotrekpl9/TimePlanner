using Domain.Primitives;

namespace Domain.Group.Models.ValueObjects;

public class GroupId : ValueObject
{
    public GroupId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}