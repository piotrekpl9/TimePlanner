using Domain.Primitives;

namespace Domain.Group.Entities;

public class Member : Entity
{
    private Member(Guid id) : base(id)
    {
    }

    public Guid UserId { get; set; }
}