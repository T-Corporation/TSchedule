using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedScheduleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_WeekDayId",
                schema: "Timetable",
                table: "Schedule",
                column: "WeekDayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_WeekDayId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "WeekDayId", "LessonId", "TeacherId" },
                unique: true);
        }
    }
}
