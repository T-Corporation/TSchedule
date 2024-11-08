using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "School");

            migrationBuilder.EnsureSchema(
                name: "Academic");

            migrationBuilder.EnsureSchema(
                name: "Timetable");

            migrationBuilder.CreateTable(
                name: "Administrators",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "Academic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "Academic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Course = table.Column<byte>(type: "tinyint", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "Academic",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "Academic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WeeklyHours = table.Column<byte>(type: "tinyint", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "Academic",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSubjects_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Academic",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalSchema: "School",
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teachers_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsentFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsentTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRegistered = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                schema: "Timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Semester = table.Column<byte>(type: "tinyint", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    IsDenominator = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    WeekDayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Academic",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_WeekDays_WeekDayId",
                        column: x => x.WeekDayId,
                        principalSchema: "Academic",
                        principalTable: "WeekDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachersPreferredTimes",
                schema: "Academic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeekId = table.Column<int>(type: "int", nullable: false),
                    PreferredStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    PreferredEnd = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersPreferredTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachersPreferredTimes_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachersPreferredTimes_WeekDays_DayOfWeekId",
                        column: x => x.DayOfWeekId,
                        principalSchema: "Academic",
                        principalTable: "WeekDays",
                        principalColumn: "Id");
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
                name: "IX_Administrators_Email",
                schema: "School",
                table: "Administrators",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_PhoneNumber",
                schema: "School",
                table: "Administrators",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_UserName",
                schema: "School",
                table: "Administrators",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherId",
                schema: "School",
                table: "Announcements",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_Number",
                schema: "School",
                table: "Classrooms",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Code",
                schema: "Academic",
                table: "Groups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SpecialtyId",
                schema: "Academic",
                table: "Groups",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_GroupId",
                table: "GroupSubjects",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_SubjectId",
                table: "GroupSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_StartTime_EndTime",
                table: "Lesson",
                columns: new[] { "StartTime", "EndTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_GroupId",
                schema: "Timetable",
                table: "Schedule",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_LessonId",
                schema: "Timetable",
                table: "Schedule",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeacherId",
                schema: "Timetable",
                table: "Schedule",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_WeekDayId",
                schema: "Timetable",
                table: "Schedule",
                column: "WeekDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Code",
                schema: "Academic",
                table: "Specialties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Code",
                schema: "Academic",
                table: "Subjects",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name",
                schema: "Academic",
                table: "Subjects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SpecialtyId",
                schema: "Academic",
                table: "Subjects",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ClassroomId",
                schema: "School",
                table: "Teachers",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                schema: "School",
                table: "Teachers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_PhoneNumber",
                schema: "School",
                table: "Teachers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SubjectId",
                schema: "School",
                table: "Teachers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersPreferredTimes_DayOfWeekId",
                schema: "Academic",
                table: "TeachersPreferredTimes",
                column: "DayOfWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersPreferredTimes_TeacherId_DayOfWeekId",
                schema: "Academic",
                table: "TeachersPreferredTimes",
                columns: new[] { "TeacherId", "DayOfWeekId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Announcements",
                schema: "School");

            migrationBuilder.DropTable(
                name: "GroupSubjects");

            migrationBuilder.DropTable(
                name: "Schedule",
                schema: "Timetable");

            migrationBuilder.DropTable(
                name: "TeachersPreferredTimes",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "School");

            migrationBuilder.DropTable(
                name: "WeekDays",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Classrooms",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Specialties",
                schema: "Academic");
        }
    }
}
