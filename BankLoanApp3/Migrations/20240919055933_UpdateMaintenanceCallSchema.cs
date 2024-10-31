using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanApp3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaintenanceCallSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "c1039367-0d5f-4ecd-999a-2f70ae1c0828");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 19, 7, 46, 2, 123, DateTimeKind.Local).AddTicks(1208));
        }
    }
}
