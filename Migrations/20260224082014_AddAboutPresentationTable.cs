using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class AddAboutPresentationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutPresentations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutPresentations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutPresentationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutPresentationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutPresentationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutPresentationTranslations_AboutPresentations_AboutPresentationId",
                        column: x => x.AboutPresentationId,
                        principalTable: "AboutPresentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutPresentationTranslations_AboutPresentationId",
                table: "AboutPresentationTranslations",
                column: "AboutPresentationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutPresentationTranslations");

            migrationBuilder.DropTable(
                name: "AboutPresentations");
        }
    }
}
