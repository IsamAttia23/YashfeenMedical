using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YashfeenMedical.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixuserClaimstableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UerClaims_Users_UserId",
                schema: "security",
                table: "UerClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UerClaims",
                schema: "security",
                table: "UerClaims");

            migrationBuilder.RenameTable(
                name: "UerClaims",
                schema: "security",
                newName: "UesrClaims",
                newSchema: "security");

            migrationBuilder.RenameIndex(
                name: "IX_UerClaims_UserId",
                schema: "security",
                table: "UesrClaims",
                newName: "IX_UesrClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UesrClaims",
                schema: "security",
                table: "UesrClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UesrClaims_Users_UserId",
                schema: "security",
                table: "UesrClaims",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UesrClaims_Users_UserId",
                schema: "security",
                table: "UesrClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UesrClaims",
                schema: "security",
                table: "UesrClaims");

            migrationBuilder.RenameTable(
                name: "UesrClaims",
                schema: "security",
                newName: "UerClaims",
                newSchema: "security");

            migrationBuilder.RenameIndex(
                name: "IX_UesrClaims_UserId",
                schema: "security",
                table: "UerClaims",
                newName: "IX_UerClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UerClaims",
                schema: "security",
                table: "UerClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UerClaims_Users_UserId",
                schema: "security",
                table: "UerClaims",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
