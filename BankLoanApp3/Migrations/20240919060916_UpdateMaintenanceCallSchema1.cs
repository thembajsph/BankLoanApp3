using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanApp3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaintenanceCallSchema1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "3e11a534-3916-452c-8d19-3cec08061c6e");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 19, 8, 9, 16, 327, DateTimeKind.Local).AddTicks(6718));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "ba1d21d4-2f10-4985-914d-4a2316c42fe7");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 19, 7, 59, 33, 187, DateTimeKind.Local).AddTicks(7049));
        }
    }
}
