using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YashfeenMedical.DAL.Migrations
{
    /// <inheritdoc />
    public partial class applyrowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RowVerison",
                table: "Patients",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "RowVerison",
                table: "Invoices",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "RowVerison",
                table: "Doctors",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "RowVerison",
                table: "Appointments",
                newName: "RowVersion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Patients",
                newName: "RowVerison");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Invoices",
                newName: "RowVerison");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Doctors",
                newName: "RowVerison");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Appointments",
                newName: "RowVerison");
        }
    }
}
