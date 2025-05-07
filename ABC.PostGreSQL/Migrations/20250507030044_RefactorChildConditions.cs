using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorChildConditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "Children");

            migrationBuilder.CreateTable(
                name: "ChildConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChildChildCondition",
                columns: table => new
                {
                    ConditionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    childrenId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildChildCondition", x => new { x.ConditionsId, x.childrenId });
                    table.ForeignKey(
                        name: "FK_ChildChildCondition_ChildConditions_ConditionsId",
                        column: x => x.ConditionsId,
                        principalTable: "ChildConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildChildCondition_Children_childrenId",
                        column: x => x.childrenId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildChildCondition_childrenId",
                table: "ChildChildCondition",
                column: "childrenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildChildCondition");

            migrationBuilder.DropTable(
                name: "ChildConditions");

            migrationBuilder.AddColumn<string>(
                name: "Conditions",
                table: "Children",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
