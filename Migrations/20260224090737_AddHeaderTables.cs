using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class AddHeaderTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoursesHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FivePProgramHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FivePProgramHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SummerSchoolHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummerSchoolHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryHeaderTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryHeaderTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryHeaderTranslations_CountryHeaders_CountryHeaderId",
                        column: x => x.CountryHeaderId,
                        principalTable: "CountryHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursesHeaderTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoursesHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesHeaderTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursesHeaderTranslations_CoursesHeaders_CoursesHeaderId",
                        column: x => x.CoursesHeaderId,
                        principalTable: "CoursesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FivePProgramHeaderTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FivePProgramHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FivePProgramHeaderTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FivePProgramHeaderTranslations_FivePProgramHeaders_FivePProgramHeaderId",
                        column: x => x.FivePProgramHeaderId,
                        principalTable: "FivePProgramHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummerSchoolHeaderTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummerSchoolHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummerSchoolHeaderTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummerSchoolHeaderTranslations_SummerSchoolHeaders_SummerSchoolHeaderId",
                        column: x => x.SummerSchoolHeaderId,
                        principalTable: "SummerSchoolHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryHeaderTranslations_CountryHeaderId",
                table: "CountryHeaderTranslations",
                column: "CountryHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesHeaderTranslations_CoursesHeaderId",
                table: "CoursesHeaderTranslations",
                column: "CoursesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FivePProgramHeaderTranslations_FivePProgramHeaderId",
                table: "FivePProgramHeaderTranslations",
                column: "FivePProgramHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SummerSchoolHeaderTranslations_SummerSchoolHeaderId",
                table: "SummerSchoolHeaderTranslations",
                column: "SummerSchoolHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryHeaderTranslations");

            migrationBuilder.DropTable(
                name: "CoursesHeaderTranslations");

            migrationBuilder.DropTable(
                name: "FivePProgramHeaderTranslations");

            migrationBuilder.DropTable(
                name: "SummerSchoolHeaderTranslations");

            migrationBuilder.DropTable(
                name: "CountryHeaders");

            migrationBuilder.DropTable(
                name: "CoursesHeaders");

            migrationBuilder.DropTable(
                name: "FivePProgramHeaders");

            migrationBuilder.DropTable(
                name: "SummerSchoolHeaders");
        }
    }
}
