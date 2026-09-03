using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YashfeenMedical.DAL.Migrations
{
    /// <inheritdoc />
    public partial class refixuserClaimsTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "UserClaims",
                newSchema: "security");

            migrationBuilder.RenameIndex(
                name: "IX_UesrClaims_UserId",
                schema: "security",
                table: "UserClaims",
                newName: "IX_UserClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                schema: "security",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_UserId",
                schema: "security",
                table: "UserClaims",
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
                name: "FK_UserClaims_Users_UserId",
                schema: "security",
                table: "UserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                schema: "security",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "security",
                newName: "UesrClaims",
                newSchema: "security");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaims_UserId",
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
    }
}
