using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityTranslationsAndLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "preferredLanguage",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "entityTranslations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entityType = table.Column<int>(type: "integer", nullable: false),
                    entityId = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_entityTranslations", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "entityTranslations",
                columns: new[] { "id", "createdAt", "createdBy", "description", "entityId", "entityType", "language", "name", "updatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Un sonido fuerte repentino o inesperado en el entorno", new Guid("a6c5ca3b-6ade-4b65-b0e3-64af93caee49"), 1, "es", "Ruido fuerte", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Una solicitud o demanda fue denegada o rechazada", new Guid("30b66d14-a9e3-4037-8ec1-1f05bc7ec940"), 1, "es", "Solicitud denegada", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Transición entre actividades o lugares", new Guid("948150ee-a06f-46e2-a76a-9955ddaeb809"), 1, "es", "Transición", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Se presentó una tarea o instrucción", new Guid("45815eee-87b7-4123-9ebc-2b06913d3309"), 1, "es", "Demanda de tarea", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Se inició una interacción social con un compañero o adulto", new Guid("3ddfccd6-0a2b-485b-9333-b2e13aad51f8"), 1, "es", "Interacción social", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "El niño fue dejado sin atención o interacción", new Guid("0a44ef8e-8d7a-45af-9d0b-c243db8bcfd6"), 1, "es", "Dejado solo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Una interrupción o cambio en el horario esperado", new Guid("bdaceb17-3b6d-430b-ba49-9ea2024da58d"), 1, "es", "Cambio en la rutina", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Exposición a un estímulo sensorial específico (luz, textura, olor)", new Guid("ec3398d0-622b-4dbe-ba95-5f46f6c647b6"), 1, "es", "Estímulo sensorial", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Un objeto o actividad preferida fue retirado", new Guid("4d9a81b5-4473-43e6-936b-e1be563cec21"), 1, "es", "Objeto preferido retirado", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-00000000000a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Se requirió esperar un turno, objeto o actividad", new Guid("4ffdd6e3-f8a9-4731-adb2-dee92e675288"), 1, "es", "Espera", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-00000000000b"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Desacuerdo o conflicto con un compañero", new Guid("b2cf3462-ce01-493a-9355-93a072be45af"), 1, "es", "Conflicto con compañeros", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-00000000000c"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Estado físico de hambre, sed o cansancio", new Guid("b22744f0-206b-45aa-85b7-f53624bd1c86"), 1, "es", "Hambre o fatiga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Llanto o lagrimeo", new Guid("c25168ef-7ef7-4a8b-9c1d-351e89df4e20"), 2, "es", "Llanto", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Golpear a otros con las manos u objetos", new Guid("6d51409e-bbdc-45dc-a50f-8404c6681047"), 2, "es", "Golpear", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Patear a otros u objetos", new Guid("36404391-9bc1-4fea-879e-c33647ac0adc"), 2, "es", "Patear", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Gritar o vociferar fuertemente", new Guid("b15aef26-7008-4fe5-9448-4c72da6c1e15"), 2, "es", "Gritar", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Morderse a sí mismo o a otros", new Guid("8db0289d-0a4f-4ff6-9e80-b2d718771c9e"), 2, "es", "Morder", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Lanzar artículos o materiales", new Guid("fe85c34d-c5eb-4a59-be23-47b464198b2a"), 2, "es", "Lanzar objetos", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Abandonar el área designada sin permiso", new Guid("e18be934-2d56-423b-9cc9-f5516d08ecb0"), 2, "es", "Fuga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Negarse a seguir instrucciones o direcciones", new Guid("82f3efe1-97c3-404e-a0ca-66d240a446cd"), 2, "es", "Incumplimiento", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Participar en comportamiento autolesivo (golpearse la cabeza, rascarse)", new Guid("9af0cc72-dd3b-4784-9173-90df515c5b1e"), 2, "es", "Autolesión", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-00000000000a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Episodio prolongado de llanto, gritos y resistencia física", new Guid("4c1ef311-cace-4d45-b7b1-147b28f88d91"), 2, "es", "Berrinches", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-00000000000b"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Romper, rasgar o dañar artículos", new Guid("8f44e0f6-0d10-44bc-8c52-6c34ffb1b39e"), 2, "es", "Destrucción de propiedad", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-00000000000c"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Usar lenguaje amenazante u hostil", new Guid("eef83b9c-7852-49cd-8bc5-b99c22128334"), 2, "es", "Agresión verbal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Redirigido a una actividad o tarea diferente", new Guid("441cd995-f81e-4950-99cd-c8ffeb88358a"), 3, "es", "Redirigido", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Recibió una corrección o reprimenda verbal", new Guid("7d6054ce-0670-4eee-896a-759f79553074"), 3, "es", "Reprimenda verbal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "El comportamiento fue ignorado intencionalmente (ignorar planificado)", new Guid("aedf4064-7aeb-4e24-b690-795d9e1b7692"), 3, "es", "Ignorado", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Recibió atención (verbal, física o de proximidad)", new Guid("d8ba54d1-7417-47bc-9646-3b99090105b0"), 3, "es", "Atención recibida", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "La demanda o tarea fue eliminada o pospuesta", new Guid("4cfc6a22-e070-410c-b029-dba6129af02d"), 3, "es", "Tarea eliminada", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Se le dio acceso a un objeto o actividad preferida", new Guid("90c0e8f6-508a-479a-be5f-9d6a9e41c0e2"), 3, "es", "Acceso a objeto preferido", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Retirado del entorno reforzante temporalmente", new Guid("e7d2dfa7-daec-4e26-a509-3cef2ff08f0f"), 3, "es", "Tiempo fuera", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Recibió guía física para completar una tarea", new Guid("4575b00c-6938-4b5d-964b-666fc865fd89"), 3, "es", "Indicación física", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Los compañeros reaccionaron al comportamiento (riendo, llorando, alejándose)", new Guid("9de37eeb-5008-4fdb-b123-3cbd563b5c57"), 3, "es", "Reacción de compañeros", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-0000-0000-0000-00000000000a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Ocurrió una consecuencia natural sin intervención de un adulto", new Guid("537642d3-0608-44ae-a89a-5028ddceeb59"), 3, "es", "Consecuencia natural", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "iX_entityTranslations_entityType_entityId_language",
                table: "entityTranslations",
                columns: new[] { "entityType", "entityId", "language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entityTranslations");

            migrationBuilder.DropColumn(
                name: "preferredLanguage",
                table: "AspNetUsers");
        }
    }
}
