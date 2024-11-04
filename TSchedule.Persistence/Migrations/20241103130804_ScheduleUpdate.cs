using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "WeekDayId", "LessonId", "ClassroomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "WeekDayId", "LessonId", "TeacherId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                schema: "Timetable",
                table: "Schedule",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                schema: "Timetable",
                table: "Schedule",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "WeekDayId", "StartTime", "ClassroomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "WeekDayId", "StartTime", "TeacherId" },
                unique: true);
        }
    }
}
