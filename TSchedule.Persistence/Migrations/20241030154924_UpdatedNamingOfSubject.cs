using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNamingOfSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeeklyHours",
                schema: "Academic",
                table: "Subjects",
                newName: "SemesterHours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SemesterHours",
                schema: "Academic",
                table: "Subjects",
                newName: "WeeklyHours");
        }
    }
}
