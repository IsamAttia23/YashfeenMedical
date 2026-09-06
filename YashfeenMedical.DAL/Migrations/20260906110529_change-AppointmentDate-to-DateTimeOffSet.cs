using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YashfeenMedical.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeAppointmentDatetoDateTimeOffSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
