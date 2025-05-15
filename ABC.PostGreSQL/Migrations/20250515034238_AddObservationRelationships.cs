using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddObservationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Observation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChildId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observation_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntecedentObservation",
                columns: table => new
                {
                    AntecedentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntecedentObservation", x => new { x.AntecedentsId, x.ObservationsId });
                    table.ForeignKey(
                        name: "FK_AntecedentObservation_Antecedents_AntecedentsId",
                        column: x => x.AntecedentsId,
                        principalTable: "Antecedents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AntecedentObservation_Observation_ObservationsId",
                        column: x => x.ObservationsId,
                        principalTable: "Observation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BehaviorObservation",
                columns: table => new
                {
                    BehaviorsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorObservation", x => new { x.BehaviorsId, x.ObservationsId });
                    table.ForeignKey(
                        name: "FK_BehaviorObservation_Behaviors_BehaviorsId",
                        column: x => x.BehaviorsId,
                        principalTable: "Behaviors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehaviorObservation_Observation_ObservationsId",
                        column: x => x.ObservationsId,
                        principalTable: "Observation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsequenceObservation",
                columns: table => new
                {
                    ConsequencesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsequenceObservation", x => new { x.ConsequencesId, x.ObservationsId });
                    table.ForeignKey(
                        name: "FK_ConsequenceObservation_Consequences_ConsequencesId",
                        column: x => x.ConsequencesId,
                        principalTable: "Consequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsequenceObservation_Observation_ObservationsId",
                        column: x => x.ObservationsId,
                        principalTable: "Observation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentObservation_ObservationsId",
                table: "AntecedentObservation",
                column: "ObservationsId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorObservation_ObservationsId",
                table: "BehaviorObservation",
                column: "ObservationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsequenceObservation_ObservationsId",
                table: "ConsequenceObservation",
                column: "ObservationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_ChildId",
                table: "Observation",
                column: "ChildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntecedentObservation");

            migrationBuilder.DropTable(
                name: "BehaviorObservation");

            migrationBuilder.DropTable(
                name: "ConsequenceObservation");

            migrationBuilder.DropTable(
                name: "Observation");
        }
    }
}
