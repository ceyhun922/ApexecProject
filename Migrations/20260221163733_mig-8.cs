using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class mig8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_EducationLevels_EducationLevelId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentTranslation_Department_DepartmentId",
                table: "DepartmentTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentTranslation",
                table: "DepartmentTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "DepartmentTranslation",
                newName: "DepartmentTranslations");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentTranslation_DepartmentId",
                table: "DepartmentTranslations",
                newName: "IX_DepartmentTranslations_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Department_EducationLevelId",
                table: "Departments",
                newName: "IX_Departments_EducationLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentTranslations",
                table: "DepartmentTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_EducationLevels_EducationLevelId",
                table: "Departments",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentTranslations_Departments_DepartmentId",
                table: "DepartmentTranslations",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_EducationLevels_EducationLevelId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentTranslations_Departments_DepartmentId",
                table: "DepartmentTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentTranslations",
                table: "DepartmentTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "DepartmentTranslations",
                newName: "DepartmentTranslation");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentTranslations_DepartmentId",
                table: "DepartmentTranslation",
                newName: "IX_DepartmentTranslation_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_EducationLevelId",
                table: "Department",
                newName: "IX_Department_EducationLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentTranslation",
                table: "DepartmentTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_EducationLevels_EducationLevelId",
                table: "Department",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentTranslation_Department_DepartmentId",
                table: "DepartmentTranslation",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
