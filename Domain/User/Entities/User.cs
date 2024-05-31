using Domain.Primitives;
using Domain.Shared;
using Domain.User.Errors;
using Domain.User.ValueObjects;

namespace Domain.User.Entities;

public sealed class User : Entity<UserId>
{
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private User(
        UserId id, 
        string name,
        string surname,
        string email, 
        string password, 
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
    }

    public static User Create(
        string name,
        string surname,
        string email, 
        string password
        )
    {
        return new User(
            new UserId(Guid.NewGuid()),
            name,
            surname, 
            email, 
            password,
            DateTime.UtcNow,
            null,
            null
            );
    }

    public void SetPassword(string password)
    {
        Password = password;
    }
    public Result Delete()
    {
        if (DeletedAt.HasValue)
        {
            return Result.Failure(UserError.AlreadyDeleted);
        }
        
        DeletedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
}