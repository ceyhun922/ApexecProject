using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class mig14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Contacts");
        }
    }
}
