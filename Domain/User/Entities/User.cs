using Domain.Primitives;

namespace Domain.User.Entities;

public sealed class User : Entity
{
    public User(Guid id) : base(id)
    {
    }
}