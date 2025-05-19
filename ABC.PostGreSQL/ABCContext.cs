using System.Text.Json;
using ABC.Management.Domain.Entities;
using ABC.Management.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL;

public class ABCContext(DbContextOptions<ABCContext> options) : DbContext(options)
{
    public DbSet<Antecedent> Antecedents => Set<Antecedent>();
    public DbSet<Behavior> Behaviors => Set<Behavior>();
    public DbSet<Consequence> Consequences => Set<Consequence>();
    public DbSet<Child> Children => Set<Child>();
    public DbSet<ChildCondition> ChildConditions => Set<ChildCondition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        modelBuilder.Entity<ChildCondition>().HasKey(x => x.Id);
        modelBuilder.Entity<Antecedent>().HasKey(e => e.Id);
        modelBuilder.Entity<Behavior>().HasKey(e => e.Id);
        modelBuilder.Entity<Consequence>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>()
            .HasMany(c => c.Conditions)
            .WithMany(c => c.children);

        modelBuilder.Entity<Observation>().HasKey(e => e.Id);
        modelBuilder.Entity<Observation>()
            .HasMany(o => o.Antecedents)
            .WithMany(a => a.Observations);

        modelBuilder.Entity<Observation>()
            .HasOne(o => o.Child)
            .WithMany(c => c.Observations);

        modelBuilder.Entity<Observation>()
            .HasMany(o => o.Behaviors)
            .WithMany(a => a.Observations);

        modelBuilder.Entity<Observation>()
            .HasMany(o => o.Consequences)
            .WithMany(a => a.Observations);

        modelBuilder.Entity<Observation>()
            .Property(o => o.When)
            .HasColumnType("jsonb");

    }
}
