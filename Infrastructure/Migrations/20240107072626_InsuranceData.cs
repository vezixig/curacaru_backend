using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsuranceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Companies_CompanyId",
                table: "Insurances");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Insurances",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Insurances",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Insurances",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_ZipCode",
                table: "Insurances",
                column: "ZipCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Companies_CompanyId",
                table: "Insurances",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_ZipCities_ZipCode",
                table: "Insurances",
                column: "ZipCode",
                principalTable: "ZipCities",
                principalColumn: "ZipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Companies_CompanyId",
                table: "Insurances");

            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_ZipCities_ZipCode",
                table: "Insurances");

            migrationBuilder.DropIndex(
                name: "IX_Insurances_ZipCode",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Insurances");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Insurances",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Companies_CompanyId",
                table: "Insurances",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
