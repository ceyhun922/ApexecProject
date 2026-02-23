using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class migcontactheaderusernames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Contacts",
                newName: "XUsername");

            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "Contacts",
                newName: "LnUsername");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Contacts",
                newName: "InstaUsername");

            migrationBuilder.AddColumn<string>(
                name: "FbUsername",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactHeaderTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactHeaderTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactHeaderTranslations_ContactHeaders_ContactHeaderId",
                        column: x => x.ContactHeaderId,
                        principalTable: "ContactHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactHeaderTranslations_ContactHeaderId",
                table: "ContactHeaderTranslations",
                column: "ContactHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactHeaderTranslations");

            migrationBuilder.DropTable(
                name: "ContactHeaders");

            migrationBuilder.DropColumn(
                name: "FbUsername",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "XUsername",
                table: "Contacts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "LnUsername",
                table: "Contacts",
                newName: "SubTitle");

            migrationBuilder.RenameColumn(
                name: "InstaUsername",
                table: "Contacts",
                newName: "ImageUrl");
        }
    }
}
