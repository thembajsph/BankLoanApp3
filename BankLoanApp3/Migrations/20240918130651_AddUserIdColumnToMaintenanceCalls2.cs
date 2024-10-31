using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanApp3.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumnToMaintenanceCalls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "3f3e5306-656f-4788-b7cc-6ed0c8bb2ad1");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 18, 15, 6, 50, 992, DateTimeKind.Local).AddTicks(414));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "b9d8f4d8-4193-444a-95e6-fae3c43d9b42");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 18, 14, 30, 8, 768, DateTimeKind.Local).AddTicks(6370));
        }
    }
}
