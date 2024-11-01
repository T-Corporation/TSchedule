using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleLessonNumber : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Teachers_TeacherId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.AddColumn<byte>(
                name: "LessonNumber",
                schema: "Timetable",
                table: "Schedule",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "DayOfWeek", "StartTime", "ClassroomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule",
                columns: new[] { "DayOfWeek", "StartTime", "TeacherId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Classrooms_ClassroomId",
                schema: "Timetable",
                table: "Schedule",
                column: "ClassroomId",
                principalSchema: "School",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Teachers_TeacherId",
                schema: "Timetable",
                table: "Schedule",
                column: "TeacherId",
                principalSchema: "School",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Classrooms_ClassroomId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Teachers_TeacherId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "LessonNumber",
                schema: "Timetable",
                table: "Schedule");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Teachers_TeacherId",
                schema: "Timetable",
                table: "Schedule",
                column: "TeacherId",
                principalSchema: "School",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
