using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentDeclarationCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDeclarations_CompanyId",
                table: "AssignmentDeclarations",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentDeclarations_Companies_CompanyId",
                table: "AssignmentDeclarations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentDeclarations_Companies_CompanyId",
                table: "AssignmentDeclarations");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentDeclarations_CompanyId",
                table: "AssignmentDeclarations");
        }
    }
}
