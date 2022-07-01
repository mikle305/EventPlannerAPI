using Microsoft.EntityFrameworkCore;

namespace EventPlannerAPI.Data;

public class DataContext: DbContext
{
    public DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // Create, if doesn't exist
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(u => new {u.Email, u.Username}).IsUnique();
    }
}