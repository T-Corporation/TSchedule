using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedStructureOnceAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_StudentGroups_GroupId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Specialties_SpecialtyId",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Specialties_SpecialtyId",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Workload_StudentGroups_GroupId",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropForeignKey(
                name: "FK_Workload_Subjects_SubjectId",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropIndex(
                name: "IX_Workload_GroupId",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropIndex(
                name: "IX_Workload_SubjectId",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SpecialtyId",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentGroups",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_SpecialtyId",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialties",
                schema: "Academic",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_GroupId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_SubjectId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropColumn(
                name: "Classroom",
                schema: "School",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Academic",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.RenameColumn(
                name: "GroupCode",
                schema: "Academic",
                table: "StudentGroups",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "Timetable",
                table: "Workload",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectCode",
                schema: "Timetable",
                table: "Workload",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Academic",
                table: "Subjects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyCode",
                schema: "Academic",
                table: "Subjects",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<byte>(
                name: "Course",
                schema: "Academic",
                table: "StudentGroups",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "Timetable",
                table: "Schedule",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubjectCode",
                schema: "Timetable",
                table: "Schedule",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                schema: "Academic",
                table: "Subjects",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentGroups",
                schema: "Academic",
                table: "StudentGroups",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialties",
                schema: "Academic",
                table: "Specialties",
                column: "Code");

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
                name: "IX_Subjects_SpecialtyCode",
                schema: "Academic",
                table: "Subjects",
                column: "SpecialtyCode");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups",
                column: "SpecialtyCode");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_StudentGroups_GroupCode",
                schema: "Timetable",
                table: "Schedule",
                column: "GroupCode",
                principalSchema: "Academic",
                principalTable: "StudentGroups",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subjects_SubjectCode",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectCode",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Specialties_SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups",
                column: "SpecialtyCode",
                principalSchema: "Academic",
                principalTable: "Specialties",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Specialties_SpecialtyCode",
                schema: "Academic",
                table: "Subjects",
                column: "SpecialtyCode",
                principalSchema: "Academic",
                principalTable: "Specialties",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workload_StudentGroups_GroupCode",
                schema: "Timetable",
                table: "Workload",
                column: "GroupCode",
                principalSchema: "Academic",
                principalTable: "StudentGroups",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Workload_Subjects_SubjectCode",
                schema: "Timetable",
                table: "Workload",
                column: "SubjectCode",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_StudentGroups_GroupCode",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Subjects_SubjectCode",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Specialties_SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Specialties_SpecialtyCode",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Workload_StudentGroups_GroupCode",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropForeignKey(
                name: "FK_Workload_Subjects_SubjectCode",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropIndex(
                name: "IX_Workload_GroupCode",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropIndex(
                name: "IX_Workload_SubjectCode",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SpecialtyCode",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentGroups",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialties",
                schema: "Academic",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_GroupCode",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_SubjectCode",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropColumn(
                name: "SubjectCode",
                schema: "Timetable",
                table: "Workload");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SpecialtyCode",
                schema: "Academic",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SpecialtyCode",
                schema: "Academic",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SubjectCode",
                schema: "Timetable",
                table: "Schedule");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "Academic",
                table: "StudentGroups",
                newName: "GroupCode");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                schema: "Timetable",
                table: "Workload",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                schema: "Timetable",
                table: "Workload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Classroom",
                schema: "School",
                table: "Teachers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Academic",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                schema: "Academic",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Course",
                schema: "Academic",
                table: "StudentGroups",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Academic",
                table: "StudentGroups",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                schema: "Academic",
                table: "StudentGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Academic",
                table: "Specialties",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                schema: "Academic",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentGroups",
                schema: "Academic",
                table: "StudentGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialties",
                schema: "Academic",
                table: "Specialties",
                column: "Id");

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
                name: "IX_Subjects_SpecialtyId",
                schema: "Academic",
                table: "Subjects",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_SpecialtyId",
                schema: "Academic",
                table: "StudentGroups",
                column: "SpecialtyId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_StudentGroups_GroupId",
                schema: "Timetable",
                table: "Schedule",
                column: "GroupId",
                principalSchema: "Academic",
                principalTable: "StudentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subjects_SubjectId",
                schema: "Timetable",
                table: "Schedule",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Specialties_SpecialtyId",
                schema: "Academic",
                table: "StudentGroups",
                column: "SpecialtyId",
                principalSchema: "Academic",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Specialties_SpecialtyId",
                schema: "Academic",
                table: "Subjects",
                column: "SpecialtyId",
                principalSchema: "Academic",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workload_StudentGroups_GroupId",
                schema: "Timetable",
                table: "Workload",
                column: "GroupId",
                principalSchema: "Academic",
                principalTable: "StudentGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workload_Subjects_SubjectId",
                schema: "Timetable",
                table: "Workload",
                column: "SubjectId",
                principalSchema: "Academic",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
