using System.Text.Json;
using ABC.Management.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL;

public class ABCContext(DbContextOptions<ABCContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Antecedent> Antecedents => Set<Antecedent>();
    public DbSet<Behavior> Behaviors => Set<Behavior>();
    public DbSet<Consequence> Consequences => Set<Consequence>();
    public DbSet<Child> Children => Set<Child>();
    public DbSet<ChildCondition> ChildConditions => Set<ChildCondition>();
    public DbSet<Observation> Observations => Set<Observation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChildCondition>().HasKey(x => x.Id);
        modelBuilder.Entity<Antecedent>().HasKey(e => e.Id);
        modelBuilder.Entity<Behavior>().HasKey(e => e.Id);
        modelBuilder.Entity<Consequence>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>()
            .HasMany(c => c.Conditions)
            .WithMany(c => c.children);
        modelBuilder.Entity<Child>()
            .HasIndex(c => c.UserId);

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
            .OwnsOne(o => o.When, d =>
            {
                d.ToJson();
            });

        modelBuilder.Entity<Observation>()
            .OwnsOne(o => o.DailyContext, d =>
            {
                d.ToJson();
            });

        SeedAntecedents(modelBuilder);
        SeedBehaviors(modelBuilder);
        SeedConsequences(modelBuilder);
    }

    private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static void SeedAntecedents(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Antecedent>().HasData(
            new { Id = Guid.Parse("a6c5ca3b-6ade-4b65-b0e3-64af93caee49"), Name = "Loud noise", Description = "A sudden or unexpected loud sound in the environment", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("30b66d14-a9e3-4037-8ec1-1f05bc7ec940"), Name = "Denied request", Description = "A request or demand was denied or refused", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("948150ee-a06f-46e2-a76a-9955ddaeb809"), Name = "Transition", Description = "Transitioning between activities or locations", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("45815eee-87b7-4123-9ebc-2b06913d3309"), Name = "Task demand", Description = "A task or instruction was presented", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("3ddfccd6-0a2b-485b-9333-b2e13aad51f8"), Name = "Social interaction", Description = "Peer or adult social engagement initiated", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("0a44ef8e-8d7a-45af-9d0b-c243db8bcfd6"), Name = "Left alone", Description = "Child was left without attention or interaction", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("bdaceb17-3b6d-430b-ba49-9ea2024da58d"), Name = "Change in routine", Description = "A disruption or change to the expected schedule", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("ec3398d0-622b-4dbe-ba95-5f46f6c647b6"), Name = "Sensory stimulus", Description = "Exposure to a specific sensory input (light, texture, smell)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("4d9a81b5-4473-43e6-936b-e1be563cec21"), Name = "Preferred item removed", Description = "A preferred item or activity was taken away", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("4ffdd6e3-f8a9-4731-adb2-dee92e675288"), Name = "Waiting", Description = "Required to wait for a turn, item, or activity", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("b2cf3462-ce01-493a-9355-93a072be45af"), Name = "Peer conflict", Description = "Disagreement or conflict with a peer", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("b22744f0-206b-45aa-85b7-f53624bd1c86"), Name = "Hunger or fatigue", Description = "Physical state of hunger, thirst, or tiredness", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" }
        );
    }

    private static void SeedBehaviors(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Behavior>().HasData(
            new { Id = Guid.Parse("c25168ef-7ef7-4a8b-9c1d-351e89df4e20"), Name = "Crying", Description = "Crying or tearfulness", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("6d51409e-bbdc-45dc-a50f-8404c6681047"), Name = "Hitting", Description = "Hitting others with hands or objects", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("36404391-9bc1-4fea-879e-c33647ac0adc"), Name = "Kicking", Description = "Kicking others or objects", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("b15aef26-7008-4fe5-9448-4c72da6c1e15"), Name = "Screaming", Description = "Screaming or yelling loudly", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("8db0289d-0a4f-4ff6-9e80-b2d718771c9e"), Name = "Biting", Description = "Biting self or others", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("fe85c34d-c5eb-4a59-be23-47b464198b2a"), Name = "Throwing objects", Description = "Throwing items or materials", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("e18be934-2d56-423b-9cc9-f5516d08ecb0"), Name = "Elopement", Description = "Leaving the designated area without permission", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("82f3efe1-97c3-404e-a0ca-66d240a446cd"), Name = "Noncompliance", Description = "Refusing to follow instructions or directions", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("9af0cc72-dd3b-4784-9173-90df515c5b1e"), Name = "Self-injury", Description = "Engaging in self-injurious behavior (head-banging, scratching)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("4c1ef311-cace-4d45-b7b1-147b28f88d91"), Name = "Tantrums", Description = "Extended episode of crying, screaming, and physical resistance", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("8f44e0f6-0d10-44bc-8c52-6c34ffb1b39e"), Name = "Property destruction", Description = "Breaking, tearing, or damaging items", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("eef83b9c-7852-49cd-8bc5-b99c22128334"), Name = "Verbal aggression", Description = "Using threatening or hostile language", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" }
        );
    }

    private static void SeedConsequences(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consequence>().HasData(
            new { Id = Guid.Parse("441cd995-f81e-4950-99cd-c8ffeb88358a"), Name = "Redirected", Description = "Redirected to a different activity or task", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("7d6054ce-0670-4eee-896a-759f79553074"), Name = "Verbal reprimand", Description = "Received a verbal correction or reprimand", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("aedf4064-7aeb-4e24-b690-795d9e1b7692"), Name = "Ignored", Description = "Behavior was intentionally ignored (planned ignoring)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("d8ba54d1-7417-47bc-9646-3b99090105b0"), Name = "Given attention", Description = "Received attention (verbal, physical, or proximity)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("4cfc6a22-e070-410c-b029-dba6129af02d"), Name = "Task removed", Description = "The demand or task was removed or postponed", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("90c0e8f6-508a-479a-be5f-9d6a9e41c0e2"), Name = "Access to preferred item", Description = "Given access to a preferred item or activity", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("e7d2dfa7-daec-4e26-a509-3cef2ff08f0f"), Name = "Time-out", Description = "Removed from the reinforcing environment temporarily", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("4575b00c-6938-4b5d-964b-666fc865fd89"), Name = "Physical prompt", Description = "Received physical guidance to complete a task", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("9de37eeb-5008-4fdb-b123-3cbd563b5c57"), Name = "Peer reaction", Description = "Peers reacted to the behavior (laughing, crying, moving away)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("537642d3-0608-44ae-a89a-5028ddceeb59"), Name = "Natural consequence", Description = "A natural consequence occurred without adult intervention", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" }
        );
    }
}
