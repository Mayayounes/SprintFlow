using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sprintFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
    name: "Status",
    table: "Tasks",
    type: "int",
    nullable: false,
    defaultValue: 0);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Status",
            table: "Tasks");
        }
    }
}
