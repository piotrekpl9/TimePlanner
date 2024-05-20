using Domain.Group.Entities;
using Microsoft.EntityFrameworkCore;
namespace Application.Common.Data;
using Domain.User.Entities;
using Domain.Task.Entities;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set;  }
    DbSet<Domain.Group.Entities.Group> Groups { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}