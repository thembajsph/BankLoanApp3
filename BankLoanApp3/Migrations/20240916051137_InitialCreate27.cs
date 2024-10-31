using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanApp3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "99e82f40-6686-4348-9abe-b9a947239336");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 16, 7, 11, 37, 134, DateTimeKind.Local).AddTicks(3777));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "abe57916-3207-4100-8aea-885728012f69");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 13, 15, 16, 6, 474, DateTimeKind.Local).AddTicks(7148));
        }
    }
}
