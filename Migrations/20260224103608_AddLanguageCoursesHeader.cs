using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class AddLanguageCoursesHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageCoursesHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageCoursesHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageCoursesHeaderTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCoursesHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageCoursesHeaderTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageCoursesHeaderTranslations_LanguageCoursesHeaders_LanguageCoursesHeaderId",
                        column: x => x.LanguageCoursesHeaderId,
                        principalTable: "LanguageCoursesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCoursesHeaderTranslations_LanguageCoursesHeaderId",
                table: "LanguageCoursesHeaderTranslations",
                column: "LanguageCoursesHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageCoursesHeaderTranslations");

            migrationBuilder.DropTable(
                name: "LanguageCoursesHeaders");
        }
    }
}
