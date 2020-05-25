using Microsoft.EntityFrameworkCore.Migrations;

namespace Florix_Feedback.Migrations
{
    public partial class FeedbackAnonymousFieldsAndReportingPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Anonymous",
                table: "Feedbacks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReportingPersonEmail",
                table: "Feedbacks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportingPersonName",
                table: "Feedbacks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anonymous",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ReportingPersonEmail",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ReportingPersonName",
                table: "Feedbacks");
        }
    }
}
