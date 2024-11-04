using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedSubjectHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workload",
                schema: "Timetable");

            migrationBuilder.DropColumn(
                name: "SemesterHours",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.AddColumn<byte>(
                name: "WeeklyHours",
                schema: "Academic",
                table: "Subjects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyHours",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "SemesterHours",
                schema: "Academic",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Workload",
                schema: "Timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    SubjectId = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hours = table.Column<short>(type: "smallint", nullable: false),
                    IsForSemester = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workload_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Academic",
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Workload_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workload_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workload_GroupId",
                schema: "Timetable",
                table: "Workload",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Workload_SubjectId",
                schema: "Timetable",
                table: "Workload",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Workload_TeacherId",
                schema: "Timetable",
                table: "Workload",
                column: "TeacherId");
        }
    }
}
