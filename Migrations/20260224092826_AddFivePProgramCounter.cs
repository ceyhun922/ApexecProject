using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class AddFivePProgramCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FivePProgramCounters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count1 = table.Column<int>(type: "int", nullable: false),
                    Count2 = table.Column<int>(type: "int", nullable: false),
                    Count3 = table.Column<int>(type: "int", nullable: false),
                    Count4 = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FivePProgramCounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FivePProgramCounterTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FivePProgramCounterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FivePProgramCounterTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FivePProgramCounterTranslations_FivePProgramCounters_FivePProgramCounterId",
                        column: x => x.FivePProgramCounterId,
                        principalTable: "FivePProgramCounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FivePProgramCounterTranslations_FivePProgramCounterId",
                table: "FivePProgramCounterTranslations",
                column: "FivePProgramCounterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FivePProgramCounterTranslations");

            migrationBuilder.DropTable(
                name: "FivePProgramCounters");
        }
    }
}
