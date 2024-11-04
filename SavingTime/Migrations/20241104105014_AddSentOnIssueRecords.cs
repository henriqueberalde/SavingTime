using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingTime.Migrations
{
    /// <inheritdoc />
    public partial class AddSentOnIssueRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "IssueRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                table: "IssueRecords");
        }
    }
}
