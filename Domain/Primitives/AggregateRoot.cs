namespace Domain.Primitives;

public abstract class AggregateRoot<T> : Entity<T> where T : ValueObject
{
    protected AggregateRoot(T id) : base(id)
    {
    }
    
    
}