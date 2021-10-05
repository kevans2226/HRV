using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace HRV.Migrations
{
    public partial class InitialCreate01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserSid = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    HRV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRVs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranges",
                columns: table => new
                {
                    RangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Low = table.Column<int>(type: "int", nullable: false),
                    High = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranges", x => x.RangeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRVs");

            migrationBuilder.DropTable(
                name: "Ranges");
        }
    }
}
