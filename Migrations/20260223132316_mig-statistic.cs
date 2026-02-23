using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexWebAPI.Migrations
{
    public partial class migstatistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatisticId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticTranslations_Statistics_StatisticId",
                        column: x => x.StatisticId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticTranslations_StatisticId",
                table: "StatisticTranslations",
                column: "StatisticId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticTranslations");

            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}
