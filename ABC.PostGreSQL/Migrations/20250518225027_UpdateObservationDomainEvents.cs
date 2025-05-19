using ABC.Management.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateObservationDomainEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Observation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Observation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeRange>(
                name: "When",
                table: "Observation",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "When",
                table: "Observation");
        }
    }
}
