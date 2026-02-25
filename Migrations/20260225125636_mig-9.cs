using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class mig9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UniversityTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Universities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SummerSchoolTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SummerSchools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LanguageCourseTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LanguageCourses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UniversityTranslations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SummerSchoolTranslations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SummerSchools");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LanguageCourseTranslations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LanguageCourses");
        }
    }
}
