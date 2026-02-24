using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class RenameToVideoSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutPresentationTranslations");

            migrationBuilder.DropTable(
                name: "PresentationTranslations");

            migrationBuilder.DropTable(
                name: "AboutPresentations");

            migrationBuilder.DropTable(
                name: "Presentations");

            migrationBuilder.CreateTable(
                name: "AboutVideoSections",
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
                    table.PrimaryKey("PK_AboutVideoSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeVideoSections",
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
                    table.PrimaryKey("PK_HomeVideoSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutVideoSectionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutVideoSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutVideoSectionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutVideoSectionTranslations_AboutVideoSections_AboutVideoSectionId",
                        column: x => x.AboutVideoSectionId,
                        principalTable: "AboutVideoSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeVideoSectionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeVideoSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeVideoSectionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeVideoSectionTranslations_HomeVideoSections_HomeVideoSectionId",
                        column: x => x.HomeVideoSectionId,
                        principalTable: "HomeVideoSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutVideoSectionTranslations_AboutVideoSectionId",
                table: "AboutVideoSectionTranslations",
                column: "AboutVideoSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeVideoSectionTranslations_HomeVideoSectionId",
                table: "HomeVideoSectionTranslations",
                column: "HomeVideoSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutVideoSectionTranslations");

            migrationBuilder.DropTable(
                name: "HomeVideoSectionTranslations");

            migrationBuilder.DropTable(
                name: "AboutVideoSections");

            migrationBuilder.DropTable(
                name: "HomeVideoSections");

            migrationBuilder.CreateTable(
                name: "AboutPresentations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutPresentations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Presentations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutPresentationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutPresentationId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "PresentationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresentationId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationTranslations_Presentations_PresentationId",
                        column: x => x.PresentationId,
                        principalTable: "Presentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutPresentationTranslations_AboutPresentationId",
                table: "AboutPresentationTranslations",
                column: "AboutPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationTranslations_PresentationId",
                table: "PresentationTranslations",
                column: "PresentationId");
        }
    }
}
