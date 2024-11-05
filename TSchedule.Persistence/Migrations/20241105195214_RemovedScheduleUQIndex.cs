using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedScheduleUQIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                table: "Schedule",
                schema: "Timetable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Teacher_Time",
                columns: ["WeekDayId", "LessonId", "TeacherId"],
                table: "Schedule",
                schema: "Timetable",
                unique: true);
        }
    }
}
