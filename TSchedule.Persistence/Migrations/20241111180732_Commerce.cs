using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Commerce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Commerce");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Commerce",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                schema: "Commerce",
                columns: table => new
                {
                    Key = table.Column<string>(type: "char(19)", maxLength: 19, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Licenses_Products_ProductName",
                        column: x => x.ProductName,
                        principalSchema: "Commerce",
                        principalTable: "Products",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Commerce",
                table: "Products",
                columns: new[] { "Name", "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { "com.romanjava.TSchedule", new DateTime(2024, 11, 11, 21, 7, 31, 57, DateTimeKind.Local).AddTicks(9472), "Программа для ведения расписания в образовательных учреждениях среднего и высшего образований.", new DateTime(2024, 11, 11, 21, 7, 31, 57, DateTimeKind.Local).AddTicks(9483) });

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_ProductName",
                schema: "Commerce",
                table: "Licenses",
                column: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses",
                schema: "Commerce");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Commerce");
        }
    }
}
