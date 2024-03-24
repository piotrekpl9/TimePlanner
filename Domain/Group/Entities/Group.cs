using Domain.Primitives;

namespace Domain.Group.Entities;

public class Group : Entity
{
    private List<Task> _tasks;
    private List<Invitation> _invitations;
    public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();
    public IReadOnlyCollection<Invitation> Invitations => _invitations.AsReadOnly();
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public User.Entities.User Creator { get; set; }
    
    public Group(Guid id, List<Task> tasks, List<Invitation> invitations, DateTime createdAt,DateTime? updatedAt,DateTime? deletedAt, User.Entities.User creator) : base(id)
    {
        _tasks = tasks;
        _invitations = invitations;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
        Creator = creator;
    }
    
}