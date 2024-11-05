using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUselessFieldsFromSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Classrooms_ClassroomId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_ClassroomId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_SubjectId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "Timetable",
                table: "Schedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                schema: "Timetable",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                schema: "Timetable",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "WeekDayId", "LessonId", "ClassroomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ClassroomId",
                schema: "Timetable",
                table: "Schedule",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SubjectId",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Classrooms_ClassroomId",
                schema: "Timetable",
                table: "Schedule",
                column: "ClassroomId",
                principalSchema: "School",
                principalTable: "Classrooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id");
        }
    }
}
