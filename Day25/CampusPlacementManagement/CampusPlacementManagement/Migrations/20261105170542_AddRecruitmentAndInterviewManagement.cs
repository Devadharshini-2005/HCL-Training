using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusPlacementManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentAndInterviewManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewRounds_RecruitmentDrives_RecruitmentDriveId",
                table: "InterviewRounds");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "InterviewSchedules",
                newName: "Venue");

            migrationBuilder.RenameColumn(
                name: "RecruitmentDriveId",
                table: "InterviewRounds",
                newName: "JobOpeningId");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewRounds_RecruitmentDriveId",
                table: "InterviewRounds",
                newName: "IX_InterviewRounds_JobOpeningId");

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "RecruitmentDrives",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RecruitmentDrives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RecruitmentDrives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "InterviewSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewRounds_JobOpenings_JobOpeningId",
                table: "InterviewRounds",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewRounds_JobOpenings_JobOpeningId",
                table: "InterviewRounds");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RecruitmentDrives");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RecruitmentDrives");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "InterviewSchedules");

            migrationBuilder.RenameColumn(
                name: "Venue",
                table: "InterviewSchedules",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "JobOpeningId",
                table: "InterviewRounds",
                newName: "RecruitmentDriveId");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewRounds_JobOpeningId",
                table: "InterviewRounds",
                newName: "IX_InterviewRounds_RecruitmentDriveId");

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "RecruitmentDrives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewRounds_RecruitmentDrives_RecruitmentDriveId",
                table: "InterviewRounds",
                column: "RecruitmentDriveId",
                principalTable: "RecruitmentDrives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
