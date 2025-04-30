using ABC.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase            
        };

        modelBuilder.Entity<Antecedent>().HasKey(e => e.Id);
        modelBuilder.Entity<Behavior>().HasKey(e => e.Id);
        modelBuilder.Entity<Consequence>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>()
            .Property(c => c.Conditions)
            .HasColumnType("jsonb")
            .HasConversion(
                l => JsonSerializer.Serialize(l, jsonOptions),
                str => JsonSerializer.Deserialize<List<string>>(str, jsonOptions) ?? new List<string>()
            );
    }
}
