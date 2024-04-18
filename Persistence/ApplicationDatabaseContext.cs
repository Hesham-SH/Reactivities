using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Activity> Activities { get; set; }
}
