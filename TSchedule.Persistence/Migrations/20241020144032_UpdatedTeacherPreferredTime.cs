using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTeacherPreferredTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredTime",
                schema: "School",
                table: "Teachers");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PreferredTimeEnd",
                schema: "School",
                table: "Teachers",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PreferredTimeStart",
                schema: "School",
                table: "Teachers",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredTimeEnd",
                schema: "School",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PreferredTimeStart",
                schema: "School",
                table: "Teachers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PreferredTime",
                schema: "School",
                table: "Teachers",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
