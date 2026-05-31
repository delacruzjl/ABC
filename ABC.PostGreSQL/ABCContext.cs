using System.Text.Json;
using ABC.Management.Domain.Entities;
using ABC.SharedKernel.Enums;
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
    public DbSet<EntityTranslation> EntityTranslations => Set<EntityTranslation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChildCondition>().HasKey(x => x.Id);
        modelBuilder.Entity<Antecedent>().HasKey(e => e.Id);
        modelBuilder.Entity<Behavior>().HasKey(e => e.Id);
        modelBuilder.Entity<Consequence>().HasKey(e => e.Id);
        modelBuilder.Entity<Child>().HasKey(e => e.Id);
        modelBuilder.Entity<EntityTranslation>().HasKey(e => e.Id);
        modelBuilder.Entity<EntityTranslation>()
            .Property(e => e.EntityType)
            .HasConversion<int>();
        modelBuilder.Entity<EntityTranslation>()
            .HasIndex(e => new { e.EntityType, e.EntityId, e.Language })
            .IsUnique();
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
        SeedSpanishTranslations(modelBuilder);
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

    private static void SeedSpanishTranslations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityTranslation>().HasData(
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000001"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("a6c5ca3b-6ade-4b65-b0e3-64af93caee49"), Language = "es", Name = "Ruido fuerte", Description = "Un sonido fuerte repentino o inesperado en el entorno", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000002"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("30b66d14-a9e3-4037-8ec1-1f05bc7ec940"), Language = "es", Name = "Solicitud denegada", Description = "Una solicitud o demanda fue denegada o rechazada", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000003"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("948150ee-a06f-46e2-a76a-9955ddaeb809"), Language = "es", Name = "Transición", Description = "Transición entre actividades o lugares", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000004"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("45815eee-87b7-4123-9ebc-2b06913d3309"), Language = "es", Name = "Demanda de tarea", Description = "Se presentó una tarea o instrucción", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000005"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("3ddfccd6-0a2b-485b-9333-b2e13aad51f8"), Language = "es", Name = "Interacción social", Description = "Se inició una interacción social con un compañero o adulto", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000006"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("0a44ef8e-8d7a-45af-9d0b-c243db8bcfd6"), Language = "es", Name = "Dejado solo", Description = "El niño fue dejado sin atención o interacción", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000007"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("bdaceb17-3b6d-430b-ba49-9ea2024da58d"), Language = "es", Name = "Cambio en la rutina", Description = "Una interrupción o cambio en el horario esperado", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000008"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("ec3398d0-622b-4dbe-ba95-5f46f6c647b6"), Language = "es", Name = "Estímulo sensorial", Description = "Exposición a un estímulo sensorial específico (luz, textura, olor)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000009"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("4d9a81b5-4473-43e6-936b-e1be563cec21"), Language = "es", Name = "Objeto preferido retirado", Description = "Un objeto o actividad preferida fue retirado", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-00000000000a"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("4ffdd6e3-f8a9-4731-adb2-dee92e675288"), Language = "es", Name = "Espera", Description = "Se requirió esperar un turno, objeto o actividad", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-00000000000b"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("b2cf3462-ce01-493a-9355-93a072be45af"), Language = "es", Name = "Conflicto con compañeros", Description = "Desacuerdo o conflicto con un compañero", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("11111111-0000-0000-0000-00000000000c"), EntityType = TranslatableEntityType.Antecedent, EntityId = Guid.Parse("b22744f0-206b-45aa-85b7-f53624bd1c86"), Language = "es", Name = "Hambre o fatiga", Description = "Estado físico de hambre, sed o cansancio", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000001"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("c25168ef-7ef7-4a8b-9c1d-351e89df4e20"), Language = "es", Name = "Llanto", Description = "Llanto o lagrimeo", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000002"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("6d51409e-bbdc-45dc-a50f-8404c6681047"), Language = "es", Name = "Golpear", Description = "Golpear a otros con las manos u objetos", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000003"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("36404391-9bc1-4fea-879e-c33647ac0adc"), Language = "es", Name = "Patear", Description = "Patear a otros u objetos", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000004"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("b15aef26-7008-4fe5-9448-4c72da6c1e15"), Language = "es", Name = "Gritar", Description = "Gritar o vociferar fuertemente", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000005"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("8db0289d-0a4f-4ff6-9e80-b2d718771c9e"), Language = "es", Name = "Morder", Description = "Morderse a sí mismo o a otros", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000006"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("fe85c34d-c5eb-4a59-be23-47b464198b2a"), Language = "es", Name = "Lanzar objetos", Description = "Lanzar artículos o materiales", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000007"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("e18be934-2d56-423b-9cc9-f5516d08ecb0"), Language = "es", Name = "Fuga", Description = "Abandonar el área designada sin permiso", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000008"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("82f3efe1-97c3-404e-a0ca-66d240a446cd"), Language = "es", Name = "Incumplimiento", Description = "Negarse a seguir instrucciones o direcciones", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-000000000009"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("9af0cc72-dd3b-4784-9173-90df515c5b1e"), Language = "es", Name = "Autolesión", Description = "Participar en comportamiento autolesivo (golpearse la cabeza, rascarse)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-00000000000a"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("4c1ef311-cace-4d45-b7b1-147b28f88d91"), Language = "es", Name = "Berrinches", Description = "Episodio prolongado de llanto, gritos y resistencia física", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-00000000000b"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("8f44e0f6-0d10-44bc-8c52-6c34ffb1b39e"), Language = "es", Name = "Destrucción de propiedad", Description = "Romper, rasgar o dañar artículos", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("22222222-0000-0000-0000-00000000000c"), EntityType = TranslatableEntityType.Behavior, EntityId = Guid.Parse("eef83b9c-7852-49cd-8bc5-b99c22128334"), Language = "es", Name = "Agresión verbal", Description = "Usar lenguaje amenazante u hostil", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000001"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("441cd995-f81e-4950-99cd-c8ffeb88358a"), Language = "es", Name = "Redirigido", Description = "Redirigido a una actividad o tarea diferente", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000002"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("7d6054ce-0670-4eee-896a-759f79553074"), Language = "es", Name = "Reprimenda verbal", Description = "Recibió una corrección o reprimenda verbal", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000003"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("aedf4064-7aeb-4e24-b690-795d9e1b7692"), Language = "es", Name = "Ignorado", Description = "El comportamiento fue ignorado intencionalmente (ignorar planificado)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000004"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("d8ba54d1-7417-47bc-9646-3b99090105b0"), Language = "es", Name = "Atención recibida", Description = "Recibió atención (verbal, física o de proximidad)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000005"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("4cfc6a22-e070-410c-b029-dba6129af02d"), Language = "es", Name = "Tarea eliminada", Description = "La demanda o tarea fue eliminada o pospuesta", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000006"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("90c0e8f6-508a-479a-be5f-9d6a9e41c0e2"), Language = "es", Name = "Acceso a objeto preferido", Description = "Se le dio acceso a un objeto o actividad preferida", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000007"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("e7d2dfa7-daec-4e26-a509-3cef2ff08f0f"), Language = "es", Name = "Tiempo fuera", Description = "Retirado del entorno reforzante temporalmente", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000008"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("4575b00c-6938-4b5d-964b-666fc865fd89"), Language = "es", Name = "Indicación física", Description = "Recibió guía física para completar una tarea", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-000000000009"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("9de37eeb-5008-4fdb-b123-3cbd563b5c57"), Language = "es", Name = "Reacción de compañeros", Description = "Los compañeros reaccionaron al comportamiento (riendo, llorando, alejándose)", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" },
            new { Id = Guid.Parse("33333333-0000-0000-0000-00000000000a"), EntityType = TranslatableEntityType.Consequence, EntityId = Guid.Parse("537642d3-0608-44ae-a89a-5028ddceeb59"), Language = "es", Name = "Consecuencia natural", Description = "Ocurrió una consecuencia natural sin intervención de un adulto", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedBy = "System" }
        );
    }
}
