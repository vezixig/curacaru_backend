using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompanyCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Companies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ZipCode",
                table: "Companies",
                column: "ZipCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_ZipCities_ZipCode",
                table: "Companies",
                column: "ZipCode",
                principalTable: "ZipCities",
                principalColumn: "ZipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_ZipCities_ZipCode",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ZipCode",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
