using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSchedule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubjects_Groups_GroupId",
                schema: "Academic",
                table: "GroupSubjects");

            migrationBuilder.DropIndex(
                name: "IX_GroupSubjects_GroupId_SubjectId",
                schema: "Academic",
                table: "GroupSubjects");

            migrationBuilder.RenameTable(
                name: "GroupSubjects",
                schema: "Academic",
                newName: "GroupSubjects");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_GroupId",
                table: "GroupSubjects",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubjects_Groups_GroupId",
                table: "GroupSubjects",
                column: "GroupId",
                principalSchema: "Academic",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubjects_Groups_GroupId",
                table: "GroupSubjects");

            migrationBuilder.DropIndex(
                name: "IX_GroupSubjects_GroupId",
                table: "GroupSubjects");

            migrationBuilder.RenameTable(
                name: "GroupSubjects",
                newName: "GroupSubjects",
                newSchema: "Academic");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_GroupId_SubjectId",
                schema: "Academic",
                table: "GroupSubjects",
                columns: new[] { "GroupId", "SubjectId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubjects_Groups_GroupId",
                schema: "Academic",
                table: "GroupSubjects",
                column: "GroupId",
                principalSchema: "Academic",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
