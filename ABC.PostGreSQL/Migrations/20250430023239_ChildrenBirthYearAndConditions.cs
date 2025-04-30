using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ChildrenBirthYearAndConditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Children",
                newName: "BirthYear");

            migrationBuilder.AddColumn<IReadOnlyCollection<string>>(
                name: "Conditions",
                table: "Children",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "Children");

            migrationBuilder.RenameColumn(
                name: "BirthYear",
                table: "Children",
                newName: "Age");
        }
    }
}
