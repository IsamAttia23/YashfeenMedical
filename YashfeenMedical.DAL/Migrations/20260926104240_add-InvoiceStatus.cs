using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YashfeenMedical.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addInvoiceStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CancelledAt",
                table: "Invoices",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");
        }
    }
}
