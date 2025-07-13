using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingTime.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentToIssueRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "IssueRecords",
                type: "nvarchar(400)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "IssueRecords");
        }
    }
}
