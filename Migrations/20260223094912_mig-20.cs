using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class mig20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Heroes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Heroes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Heroes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Heroes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
