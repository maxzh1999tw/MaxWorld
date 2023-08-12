using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxWorld.Data.Migrations
{
    /// <inheritdoc />
    public partial class Exercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Field1Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Field1Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Field2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Field2Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShouldTimeFieldsAsTotal = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.ExerciseId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercise");
        }
    }
}
