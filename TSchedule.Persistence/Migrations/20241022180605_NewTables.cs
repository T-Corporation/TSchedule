using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsentFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsentTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstituteTeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVisibleToGuests = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Teachers_SubstituteTeacherId",
                        column: x => x.SubstituteTeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredAnnouncements",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    AdministratorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_Announcements_SubstituteTeacherId",
                schema: "School",
                table: "Announcements",
                column: "SubstituteTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherId",
                schema: "School",
                table: "Announcements",
                column: "TeacherId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredAnnouncements",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Announcements",
                schema: "School");
        }
    }
}
