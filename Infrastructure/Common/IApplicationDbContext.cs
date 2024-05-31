using Microsoft.EntityFrameworkCore;
using Task = Domain.Task.Entities.Task;

namespace Infrastructure.Common;

public interface IApplicationDbContext
{
    DbSet<Domain.User.Entities.User> Users { get; set;  }
    DbSet<Domain.Group.Entities.Group> Groups { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}