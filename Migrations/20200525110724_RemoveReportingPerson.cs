using Microsoft.EntityFrameworkCore.Migrations;

namespace Florix_Feedback.Migrations
{
    public partial class RemoveReportingPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportingPersonEmail",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ReportingPersonName",
                table: "Feedbacks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportingPersonEmail",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportingPersonName",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
