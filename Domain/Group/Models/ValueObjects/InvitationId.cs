using Domain.Primitives;

namespace Domain.Group.Models.ValueObjects;

public class InvitationId : ValueObject
{
    public Guid Value { get; }

    public InvitationId(Guid value)
    {
        Value = value;
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}