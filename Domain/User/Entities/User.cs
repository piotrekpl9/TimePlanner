using Domain.Primitives;
using Domain.Shared;
using Domain.User.Errors;

namespace Domain.User.Entities;

public sealed class User : Entity
{
    private List<Task> _tasks;
    public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    
    public User(
        Guid id, 
        string name,
        string surname,
        string email, 
        string password, 
        List<Task> tasks,
        DateTime createdAt, 
        DateTime? updatedAt,
        DateTime? deletedAt) : base(id)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
        _tasks = tasks;
    }

    static User Create(
        string name,
        string surname,
        string email, 
        string password
        )
    {
        return new User(
            new Guid(),
            name,
            surname, 
            email, 
            password,
            new List<Task>(),
            DateTime.UtcNow,
            null,
            null
            );
    }

    public Result Delete()
    {
        if (DeletedAt.HasValue)
        {
            return Result.Failure(UserErrors.UserAlreadyDeleted);
        }
        
        DeletedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result AssignTask(Task task)
    {
        _tasks.Add(task);
        
        return Result.Success();
    }
}