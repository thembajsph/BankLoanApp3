using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanApp3.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressFieldsToMaintenanceCall92 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "cffea718-e6d9-453f-94ad-0d59a403297f");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 17, 15, 33, 24, 879, DateTimeKind.Local).AddTicks(6541));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "sample-user-id",
                column: "ConcurrencyStamp",
                value: "5e07a319-ccbf-4d67-b6b8-75ef066a3cbb");

            migrationBuilder.UpdateData(
                table: "HomeLoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationDate",
                value: new DateTime(2024, 9, 17, 15, 25, 22, 75, DateTimeKind.Local).AddTicks(6610));
        }
    }
}
