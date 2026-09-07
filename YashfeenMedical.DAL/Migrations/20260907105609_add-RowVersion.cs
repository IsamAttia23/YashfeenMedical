using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YashfeenMedical.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_StartTime",
                table: "Appointments");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVerison",
                table: "Patients",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVerison",
                table: "Invoices",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVerison",
                table: "Doctors",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVerison",
                table: "Appointments",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_StartTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDate", "StartTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_StartTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RowVerison",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "RowVerison",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RowVerison",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "RowVerison",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_StartTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDate", "StartTime" });
        }
    }
}
