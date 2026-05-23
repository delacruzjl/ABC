using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SeedAbcDataAndDefaultChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "defaultChildId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "antecedents",
                columns: new[] { "id", "createdAt", "createdBy", "description", "name", "updatedAt" },
                values: new object[,]
                {
                    { new Guid("0a44ef8e-8d7a-45af-9d0b-c243db8bcfd6"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Child was left without attention or interaction", "Left alone", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30b66d14-a9e3-4037-8ec1-1f05bc7ec940"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "A request or demand was denied or refused", "Denied request", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3ddfccd6-0a2b-485b-9333-b2e13aad51f8"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Peer or adult social engagement initiated", "Social interaction", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("45815eee-87b7-4123-9ebc-2b06913d3309"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "A task or instruction was presented", "Task demand", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4d9a81b5-4473-43e6-936b-e1be563cec21"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "A preferred item or activity was taken away", "Preferred item removed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4ffdd6e3-f8a9-4731-adb2-dee92e675288"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Required to wait for a turn, item, or activity", "Waiting", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("948150ee-a06f-46e2-a76a-9955ddaeb809"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Transitioning between activities or locations", "Transition", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a6c5ca3b-6ade-4b65-b0e3-64af93caee49"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "A sudden or unexpected loud sound in the environment", "Loud noise", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b22744f0-206b-45aa-85b7-f53624bd1c86"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Physical state of hunger, thirst, or tiredness", "Hunger or fatigue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2cf3462-ce01-493a-9355-93a072be45af"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Disagreement or conflict with a peer", "Peer conflict", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bdaceb17-3b6d-430b-ba49-9ea2024da58d"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "A disruption or change to the expected schedule", "Change in routine", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ec3398d0-622b-4dbe-ba95-5f46f6c647b6"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Exposure to a specific sensory input (light, texture, smell)", "Sensory stimulus", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "behaviors",
                columns: new[] { "id", "createdAt", "createdBy", "description", "name", "updatedAt" },
                values: new object[,]
                {
                    { new Guid("36404391-9bc1-4fea-879e-c33647ac0adc"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Kicking others or objects", "Kicking", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4c1ef311-cace-4d45-b7b1-147b28f88d91"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Extended episode of crying, screaming, and physical resistance", "Tantrums", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("6d51409e-bbdc-45dc-a50f-8404c6681047"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Hitting others with hands or objects", "Hitting", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("82f3efe1-97c3-404e-a0ca-66d240a446cd"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Refusing to follow instructions or directions", "Noncompliance", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8db0289d-0a4f-4ff6-9e80-b2d718771c9e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Biting self or others", "Biting", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8f44e0f6-0d10-44bc-8c52-6c34ffb1b39e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Breaking, tearing, or damaging items", "Property destruction", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9af0cc72-dd3b-4784-9173-90df515c5b1e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Engaging in self-injurious behavior (head-banging, scratching)", "Self-injury", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b15aef26-7008-4fe5-9448-4c72da6c1e15"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Screaming or yelling loudly", "Screaming", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c25168ef-7ef7-4a8b-9c1d-351e89df4e20"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Crying or tearfulness", "Crying", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e18be934-2d56-423b-9cc9-f5516d08ecb0"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Leaving the designated area without permission", "Elopement", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("eef83b9c-7852-49cd-8bc5-b99c22128334"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Using threatening or hostile language", "Verbal aggression", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("fe85c34d-c5eb-4a59-be23-47b464198b2a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Throwing items or materials", "Throwing objects", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "consequences",
                columns: new[] { "id", "createdAt", "createdBy", "description", "name", "updatedAt" },
                values: new object[,]
                {
                    { new Guid("441cd995-f81e-4950-99cd-c8ffeb88358a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Redirected to a different activity or task", "Redirected", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4575b00c-6938-4b5d-964b-666fc865fd89"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Received physical guidance to complete a task", "Physical prompt", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4cfc6a22-e070-410c-b029-dba6129af02d"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "The demand or task was removed or postponed", "Task removed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("537642d3-0608-44ae-a89a-5028ddceeb59"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "A natural consequence occurred without adult intervention", "Natural consequence", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7d6054ce-0670-4eee-896a-759f79553074"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Received a verbal correction or reprimand", "Verbal reprimand", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("90c0e8f6-508a-479a-be5f-9d6a9e41c0e2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Given access to a preferred item or activity", "Access to preferred item", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9de37eeb-5008-4fdb-b123-3cbd563b5c57"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Peers reacted to the behavior (laughing, crying, moving away)", "Peer reaction", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("aedf4064-7aeb-4e24-b690-795d9e1b7692"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Behavior was intentionally ignored (planned ignoring)", "Ignored", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d8ba54d1-7417-47bc-9646-3b99090105b0"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Received attention (verbal, physical, or proximity)", "Given attention", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e7d2dfa7-daec-4e26-a509-3cef2ff08f0f"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Removed from the reinforcing environment temporarily", "Time-out", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("0a44ef8e-8d7a-45af-9d0b-c243db8bcfd6"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("30b66d14-a9e3-4037-8ec1-1f05bc7ec940"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("3ddfccd6-0a2b-485b-9333-b2e13aad51f8"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("45815eee-87b7-4123-9ebc-2b06913d3309"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("4d9a81b5-4473-43e6-936b-e1be563cec21"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("4ffdd6e3-f8a9-4731-adb2-dee92e675288"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("948150ee-a06f-46e2-a76a-9955ddaeb809"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("a6c5ca3b-6ade-4b65-b0e3-64af93caee49"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("b22744f0-206b-45aa-85b7-f53624bd1c86"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("b2cf3462-ce01-493a-9355-93a072be45af"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("bdaceb17-3b6d-430b-ba49-9ea2024da58d"));

            migrationBuilder.DeleteData(
                table: "antecedents",
                keyColumn: "id",
                keyValue: new Guid("ec3398d0-622b-4dbe-ba95-5f46f6c647b6"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("36404391-9bc1-4fea-879e-c33647ac0adc"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("4c1ef311-cace-4d45-b7b1-147b28f88d91"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("6d51409e-bbdc-45dc-a50f-8404c6681047"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("82f3efe1-97c3-404e-a0ca-66d240a446cd"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("8db0289d-0a4f-4ff6-9e80-b2d718771c9e"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("8f44e0f6-0d10-44bc-8c52-6c34ffb1b39e"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("9af0cc72-dd3b-4784-9173-90df515c5b1e"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("b15aef26-7008-4fe5-9448-4c72da6c1e15"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("c25168ef-7ef7-4a8b-9c1d-351e89df4e20"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("e18be934-2d56-423b-9cc9-f5516d08ecb0"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("eef83b9c-7852-49cd-8bc5-b99c22128334"));

            migrationBuilder.DeleteData(
                table: "behaviors",
                keyColumn: "id",
                keyValue: new Guid("fe85c34d-c5eb-4a59-be23-47b464198b2a"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("441cd995-f81e-4950-99cd-c8ffeb88358a"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("4575b00c-6938-4b5d-964b-666fc865fd89"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("4cfc6a22-e070-410c-b029-dba6129af02d"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("537642d3-0608-44ae-a89a-5028ddceeb59"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("7d6054ce-0670-4eee-896a-759f79553074"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("90c0e8f6-508a-479a-be5f-9d6a9e41c0e2"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("9de37eeb-5008-4fdb-b123-3cbd563b5c57"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("aedf4064-7aeb-4e24-b690-795d9e1b7692"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("d8ba54d1-7417-47bc-9646-3b99090105b0"));

            migrationBuilder.DeleteData(
                table: "consequences",
                keyColumn: "id",
                keyValue: new Guid("e7d2dfa7-daec-4e26-a509-3cef2ff08f0f"));

            migrationBuilder.DropColumn(
                name: "defaultChildId",
                table: "AspNetUsers");
        }
    }
}
