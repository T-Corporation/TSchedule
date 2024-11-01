using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleDayOfWeek : Migration
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
                name: "FK_TeachersPreferredTimes_DaysOfWeek_DayOfWeekId",
                schema: "Academic",
                table: "TeachersPreferredTimes");

            migrationBuilder.DropTable(
                name: "DaysOfWeek",
                schema: "Academic");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.AddColumn<int>(
                name: "WeekDayId",
                schema: "Timetable",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WeekDays",
                schema: "Academic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDays", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Academic",
                table: "WeekDays",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Понедельник" },
                    { 2, "Вторник" },
                    { 3, "Среда" },
                    { 4, "Четверг" },
                    { 5, "Пятница" },
                    { 6, "Суббота" },
                    { 7, "Воскресенье" }
                });

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
                name: "FK_Schedule_WeekDays_WeekDayId",
                schema: "Timetable",
                table: "Schedule",
                column: "WeekDayId",
                principalSchema: "Academic",
                principalTable: "WeekDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersPreferredTimes_WeekDays_DayOfWeekId",
                schema: "Academic",
                table: "TeachersPreferredTimes",
                column: "DayOfWeekId",
                principalSchema: "Academic",
                principalTable: "WeekDays",
                principalColumn: "Id");
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
                name: "FK_Schedule_WeekDays_WeekDayId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachersPreferredTimes_WeekDays_DayOfWeekId",
                schema: "Academic",
                table: "TeachersPreferredTimes");

            migrationBuilder.DropTable(
                name: "WeekDays",
                schema: "Academic");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Classroom_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Teacher_Time",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "WeekDayId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                schema: "Timetable",
                table: "Schedule",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DaysOfWeek",
                schema: "Academic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOfWeek", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Academic",
                table: "DaysOfWeek",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Понедельник" },
                    { 2, "Вторник" },
                    { 3, "Среда" },
                    { 4, "Четверг" },
                    { 5, "Пятница" },
                    { 6, "Суббота" },
                    { 7, "Воскресенье" }
                });

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachersPreferredTimes_DaysOfWeek_DayOfWeekId",
                schema: "Academic",
                table: "TeachersPreferredTimes",
                column: "DayOfWeekId",
                principalSchema: "Academic",
                principalTable: "DaysOfWeek",
                principalColumn: "Id");
        }
    }
}
