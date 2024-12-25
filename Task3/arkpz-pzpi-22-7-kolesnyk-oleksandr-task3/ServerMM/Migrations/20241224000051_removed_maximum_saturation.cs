using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerMM.Migrations
{
    /// <inheritdoc />
    public partial class removed_maximum_saturation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxOxygenLevel",
                table: "UserOptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxOxygenLevel",
                table: "UserOptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
