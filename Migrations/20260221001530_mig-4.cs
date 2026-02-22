using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_EducationLevel_EducationLevelId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_Countries_CountryId",
                table: "EducationLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevelTranslation_EducationLevel_EducationLevelId",
                table: "EducationLevelTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationLevelTranslation",
                table: "EducationLevelTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationLevel",
                table: "EducationLevel");

            migrationBuilder.RenameTable(
                name: "EducationLevelTranslation",
                newName: "EducationLevelTranslations");

            migrationBuilder.RenameTable(
                name: "EducationLevel",
                newName: "EducationLevels");

            migrationBuilder.RenameIndex(
                name: "IX_EducationLevelTranslation_EducationLevelId",
                table: "EducationLevelTranslations",
                newName: "IX_EducationLevelTranslations_EducationLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationLevel_CountryId",
                table: "EducationLevels",
                newName: "IX_EducationLevels_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationLevelTranslations",
                table: "EducationLevelTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationLevels",
                table: "EducationLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_EducationLevels_EducationLevelId",
                table: "Department",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevels_Countries_CountryId",
                table: "EducationLevels",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevelTranslations_EducationLevels_EducationLevelId",
                table: "EducationLevelTranslations",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_EducationLevels_EducationLevelId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevels_Countries_CountryId",
                table: "EducationLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevelTranslations_EducationLevels_EducationLevelId",
                table: "EducationLevelTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationLevelTranslations",
                table: "EducationLevelTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationLevels",
                table: "EducationLevels");

            migrationBuilder.RenameTable(
                name: "EducationLevelTranslations",
                newName: "EducationLevelTranslation");

            migrationBuilder.RenameTable(
                name: "EducationLevels",
                newName: "EducationLevel");

            migrationBuilder.RenameIndex(
                name: "IX_EducationLevelTranslations_EducationLevelId",
                table: "EducationLevelTranslation",
                newName: "IX_EducationLevelTranslation_EducationLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationLevels_CountryId",
                table: "EducationLevel",
                newName: "IX_EducationLevel_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationLevelTranslation",
                table: "EducationLevelTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationLevel",
                table: "EducationLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_EducationLevel_EducationLevelId",
                table: "Department",
                column: "EducationLevelId",
                principalTable: "EducationLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_Countries_CountryId",
                table: "EducationLevel",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevelTranslation_EducationLevel_EducationLevelId",
                table: "EducationLevelTranslation",
                column: "EducationLevelId",
                principalTable: "EducationLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
