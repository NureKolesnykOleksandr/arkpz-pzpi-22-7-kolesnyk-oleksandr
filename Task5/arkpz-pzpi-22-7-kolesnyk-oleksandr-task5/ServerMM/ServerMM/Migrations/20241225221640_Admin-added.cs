using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerMM.Migrations
{
    /// <inheritdoc />
    public partial class Adminadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "DateOfBirth", "Email", "EmergencyEmail", "FirstName", "Gender", "LastName", "PasswordHash" },
                values: new object[] { 1, new DateTime(2024, 12, 26, 0, 16, 39, 650, DateTimeKind.Local).AddTicks(1525), null, "oleksandr.kolesnyk@nure.ua", "oleksandr.kolesnyk@nure.ua", "Admin", "Male", "Kolesnyk", "$2a$11$Rz7MkyZFjs.YhGXJHPjkPe57lK/R2340mQApY/6gJjzWK/rlbK/RO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
