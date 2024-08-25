using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingTime.Migrations
{
    /// <inheritdoc />
    public partial class RemoveContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Context",
                table: "TimeRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Context",
                table: "TimeRecords",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
