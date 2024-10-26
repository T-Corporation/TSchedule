using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                name: "Timetable");

            migrationBuilder.EnsureSchema(
                name: "Academic");

            migrationBuilder.CreateTable(
                name: "Administrators",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
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
                    RoomNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "Academic",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PreferredTimeStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    PreferredTimeEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                schema: "Academic",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Course = table.Column<byte>(type: "tinyint", nullable: false),
                    SpecialtyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Code);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Specialties_SpecialtyCode",
                        column: x => x.SpecialtyCode,
                        principalSchema: "Academic",
                        principalTable: "Specialties",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "Academic",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WeeklyHours = table.Column<int>(type: "int", nullable: false),
                    SpecialtyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Subjects_Specialties_SpecialtyCode",
                        column: x => x.SpecialtyCode,
                        principalSchema: "Academic",
                        principalTable: "Specialties",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
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
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                        name: "FK_Schedule_StudentGroups_GroupCode",
                        column: x => x.GroupCode,
                        principalSchema: "Academic",
                        principalTable: "StudentGroups",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_Subjects_SubjectCode",
                        column: x => x.SubjectCode,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Schedule_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workload",
                schema: "Timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hours = table.Column<int>(type: "int", nullable: false),
                    IsForSemester = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workload_StudentGroups_GroupCode",
                        column: x => x.GroupCode,
                        principalSchema: "Academic",
                        principalTable: "StudentGroups",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Workload_Subjects_SubjectCode",
                        column: x => x.SubjectCode,
                        principalSchema: "Academic",
                        principalTable: "Subjects",
                        principalColumn: "Code",
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

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherId",
                schema: "School",
                table: "Announcements",
                column: "TeacherId");

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
                name: "IX_Schedule_GroupCode",
                schema: "Timetable",
                table: "Schedule",
                column: "GroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SubjectCode",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeacherId",
                schema: "Timetable",
                table: "Schedule",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups",
                column: "SpecialtyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SpecialtyCode",
                schema: "Academic",
                table: "Subjects",
                column: "SpecialtyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Workload_GroupCode",
                schema: "Timetable",
                table: "Workload",
                column: "GroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_Workload_SubjectCode",
                schema: "Timetable",
                table: "Workload",
                column: "SubjectCode");

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
                name: "RegisteredAnnouncements",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Schedule",
                schema: "Timetable");

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
                name: "Classrooms",
                schema: "School");

            migrationBuilder.DropTable(
                name: "StudentGroups",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "Academic");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Specialties",
                schema: "Academic");
        }
    }
}
