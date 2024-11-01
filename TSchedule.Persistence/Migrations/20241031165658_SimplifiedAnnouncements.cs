using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredAnnouncements",
                schema: "School");

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                schema: "School",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegistered",
                schema: "School",
                table: "Announcements");

            migrationBuilder.CreateTable(
                name: "RegisteredAnnouncements",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministratorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredAnnouncements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredAnnouncements_Administrators_AdministratorId",
                        column: x => x.AdministratorId,
                        principalSchema: "School",
                        principalTable: "Administrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredAnnouncements_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalSchema: "School",
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredAnnouncements_AdministratorId",
                schema: "School",
                table: "RegisteredAnnouncements",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredAnnouncements_AnnouncementId",
                schema: "School",
                table: "RegisteredAnnouncements",
                column: "AnnouncementId");
        }
    }
}
