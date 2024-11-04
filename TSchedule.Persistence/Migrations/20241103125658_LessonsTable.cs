using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LessonsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonNumber",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                schema: "Timetable",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Lesson",
                columns: new[] { "Id", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeOnly(10, 5, 0), new TimeOnly(8, 30, 0) },
                    { 2, new TimeOnly(11, 50, 0), new TimeOnly(10, 15, 0) },
                    { 3, new TimeOnly(14, 5, 0), new TimeOnly(12, 30, 0) },
                    { 4, new TimeOnly(15, 50, 0), new TimeOnly(14, 15, 0) },
                    { 5, new TimeOnly(17, 35, 0), new TimeOnly(16, 0, 0) },
                    { 6, new TimeOnly(19, 20, 0), new TimeOnly(17, 45, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_LessonId",
                schema: "Timetable",
                table: "Schedule",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_StartTime_EndTime",
                table: "Lesson",
                columns: new[] { "StartTime", "EndTime" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Lesson_LessonId",
                schema: "Timetable",
                table: "Schedule",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Lesson_LessonId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_LessonId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "LessonId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.AddColumn<byte>(
                name: "LessonNumber",
                schema: "Timetable",
                table: "Schedule",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
