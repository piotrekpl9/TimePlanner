using Domain.Group.Entities;
using Microsoft.EntityFrameworkCore;
namespace Application.Common.Data;
using Domain.User.Entities;

public interface ApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Invitation> Invitations { get; set; }
    DbSet<Member> Members { get; set; }
    DbSet<Task> Tasks { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}