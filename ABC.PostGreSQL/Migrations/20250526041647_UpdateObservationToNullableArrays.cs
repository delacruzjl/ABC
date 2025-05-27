using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateObservationToNullableArrays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildId",
                table: "Observations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChildId",
                table: "Observations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
