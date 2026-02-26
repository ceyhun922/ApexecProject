using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class mig10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option1Title",
                table: "PlanningTranslations");

            migrationBuilder.DropColumn(
                name: "Checkbox1",
                table: "Plannings");

            migrationBuilder.DropColumn(
                name: "Checkbox2",
                table: "Plannings");

            migrationBuilder.DropColumn(
                name: "Checkbox3",
                table: "Plannings");

            migrationBuilder.DropColumn(
                name: "Option1",
                table: "Plannings");

            migrationBuilder.DropColumn(
                name: "Option2",
                table: "Plannings");

            migrationBuilder.DropColumn(
                name: "Option3",
                table: "Plannings");

            migrationBuilder.DropColumn(
                name: "Option4",
                table: "Plannings");

            migrationBuilder.RenameColumn(
                name: "Option4Title",
                table: "PlanningTranslations",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Option3Title",
                table: "PlanningTranslations",
                newName: "SubTitle");

            migrationBuilder.RenameColumn(
                name: "Option2Title",
                table: "PlanningTranslations",
                newName: "Badge");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PlanningTranslations",
                newName: "Option4Title");

            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "PlanningTranslations",
                newName: "Option3Title");

            migrationBuilder.RenameColumn(
                name: "Badge",
                table: "PlanningTranslations",
                newName: "Option2Title");

            migrationBuilder.AddColumn<string>(
                name: "Option1Title",
                table: "PlanningTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Checkbox1",
                table: "Plannings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Checkbox2",
                table: "Plannings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Checkbox3",
                table: "Plannings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Option1",
                table: "Plannings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option2",
                table: "Plannings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option3",
                table: "Plannings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option4",
                table: "Plannings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
