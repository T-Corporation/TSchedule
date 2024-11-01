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
                    WeeklyHours = table.Column<int>(type: "int", nullable: false),
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
                schema: "Academic",
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    AbsentFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsentTo = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    DayOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Semester = table.Column<byte>(type: "tinyint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    IsDenominator = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalSchema: "School",
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Schedule_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Academic",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Schedule_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id");
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
                        name: "FK_TeachersPreferredTimes_DaysOfWeek_DayOfWeekId",
                        column: x => x.DayOfWeekId,
                        principalSchema: "Academic",
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeachersPreferredTimes_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workload",
                schema: "Timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hours = table.Column<short>(type: "smallint", nullable: false),
                    IsForSemester = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupId = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    SubjectId = table.Column<int>(type: "int", maxLength: 20, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "RegisteredAnnouncements",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    AdministratorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredAnnouncements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredAnnouncements_Administrators_AdministratorId",
                        column: x => x.AdministratorId,
                        principalSchema: "School",
                        principalTable: "Administrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredAnnouncements_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalSchema: "School",
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_GroupSubjects_GroupId_SubjectId",
                schema: "Academic",
                table: "GroupSubjects",
                columns: new[] { "GroupId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_SubjectId",
                schema: "Academic",
                table: "GroupSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredAnnouncements_AdministratorId",
                schema: "School",
                table: "RegisteredAnnouncements",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredAnnouncements_AnnouncementId",
                schema: "School",
                table: "RegisteredAnnouncements",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ClassroomId",
                schema: "Timetable",
                table: "Schedule",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_GroupId",
                schema: "Timetable",
                table: "Schedule",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SubjectId",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeacherId",
                schema: "Timetable",
                table: "Schedule",
                column: "TeacherId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupSubjects",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "RegisteredAnnouncements",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Schedule",
                schema: "Timetable");

            migrationBuilder.DropTable(
                name: "TeachersPreferredTimes",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Workload",
                schema: "Timetable");

            migrationBuilder.DropTable(
                name: "Administrators",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Announcements",
                schema: "School");

            migrationBuilder.DropTable(
                name: "DaysOfWeek",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "School");

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
