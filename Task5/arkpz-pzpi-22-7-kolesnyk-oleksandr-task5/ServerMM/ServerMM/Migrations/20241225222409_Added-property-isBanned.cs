using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerMM.Migrations
{
    /// <inheritdoc />
    public partial class AddedpropertyisBanned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isBanned",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "isBanned" },
                values: new object[] { new DateTime(2024, 12, 26, 0, 24, 8, 686, DateTimeKind.Local).AddTicks(1062), "$2a$11$xOyCrVs3nGeZmUjpbf7Zqekg7kf4q1ynKyD9UqEmvvWAOf2P4SEuO", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBanned",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2024, 12, 26, 0, 16, 39, 650, DateTimeKind.Local).AddTicks(1525), "$2a$11$Rz7MkyZFjs.YhGXJHPjkPe57lK/R2340mQApY/6gJjzWK/rlbK/RO" });
        }
    }
}
