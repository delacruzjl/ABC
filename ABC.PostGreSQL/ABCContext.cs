using ABC.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL;

public class ABCContext(DbContextOptions<ABCContext> options) : DbContext(options)
{
    public DbSet<Antecedent> Antecedents => Set<Antecedent>();
    public DbSet<Behavior> Behaviors => Set<Behavior>();
    public DbSet<Consequence> Consequences => Set<Consequence>();
    public DbSet<Child> Children => Set<Child>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Antecedent>().HasKey(e => e.Id);
        modelBuilder.Entity<Behavior>().HasKey(e => e.Id);
        modelBuilder.Entity<Consequence>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>().HasKey(e => e.Id);
    }
}
