using Application.Common.Data;
using Domain.Group.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Common;
using Domain.User.Entities;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Domain.Task.Entities.Task> Tasks { get; set; }
}