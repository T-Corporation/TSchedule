using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AnnouncementUpdatedAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubjects_Subjects_SubjectId",
                table: "GroupSubjects");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "School",
                table: "Announcements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubjects_Subjects_SubjectId",
                table: "GroupSubjects",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubjects_Subjects_SubjectId",
                table: "GroupSubjects");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "School",
                table: "Announcements");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubjects_Subjects_SubjectId",
                table: "GroupSubjects",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
