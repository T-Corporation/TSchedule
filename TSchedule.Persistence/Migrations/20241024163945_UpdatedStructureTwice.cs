using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedStructureTwice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Teachers_SubstituteTeacherId",
                schema: "School",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_SubstituteTeacherId",
                schema: "School",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "IsVisibleToGuests",
                schema: "School",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "SubstituteTeacherId",
                schema: "School",
                table: "Announcements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleToGuests",
                schema: "School",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SubstituteTeacherId",
                schema: "School",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_SubstituteTeacherId",
                schema: "School",
                table: "Announcements",
                column: "SubstituteTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Teachers_SubstituteTeacherId",
                schema: "School",
                table: "Announcements",
                column: "SubstituteTeacherId",
                principalSchema: "School",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
