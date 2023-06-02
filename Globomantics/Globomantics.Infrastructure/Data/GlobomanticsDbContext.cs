using Globomantics.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Globomantics.Infrastructure.Data;

public class GlobomanticsDbContext : DbContext
{
    public DbSet<TodoTask> TodoTasks { get; set; }
    public DbSet<Bug> Bugs { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Globomantics.db");
    }
}
