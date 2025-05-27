using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SetObservationReadWrite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntecedentObservation_Observation_ObservationsId",
                table: "AntecedentObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorObservation_Observation_ObservationsId",
                table: "BehaviorObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsequenceObservation_Observation_ObservationsId",
                table: "ConsequenceObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Observation_Children_ChildId",
                table: "Observation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Observation",
                table: "Observation");

            migrationBuilder.RenameTable(
                name: "Observation",
                newName: "Observations");

            migrationBuilder.RenameIndex(
                name: "IX_Observation_ChildId",
                table: "Observations",
                newName: "IX_Observations_ChildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Observations",
                table: "Observations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AntecedentObservation_Observations_ObservationsId",
                table: "AntecedentObservation",
                column: "ObservationsId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorObservation_Observations_ObservationsId",
                table: "BehaviorObservation",
                column: "ObservationsId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsequenceObservation_Observations_ObservationsId",
                table: "ConsequenceObservation",
                column: "ObservationsId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntecedentObservation_Observations_ObservationsId",
                table: "AntecedentObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorObservation_Observations_ObservationsId",
                table: "BehaviorObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsequenceObservation_Observations_ObservationsId",
                table: "ConsequenceObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Observations",
                table: "Observations");

            migrationBuilder.RenameTable(
                name: "Observations",
                newName: "Observation");

            migrationBuilder.RenameIndex(
                name: "IX_Observations_ChildId",
                table: "Observation",
                newName: "IX_Observation_ChildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Observation",
                table: "Observation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AntecedentObservation_Observation_ObservationsId",
                table: "AntecedentObservation",
                column: "ObservationsId",
                principalTable: "Observation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorObservation_Observation_ObservationsId",
                table: "BehaviorObservation",
                column: "ObservationsId",
                principalTable: "Observation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsequenceObservation_Observation_ObservationsId",
                table: "ConsequenceObservation",
                column: "ObservationsId",
                principalTable: "Observation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_Children_ChildId",
                table: "Observation",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
