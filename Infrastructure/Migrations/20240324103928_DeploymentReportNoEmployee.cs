using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeploymentReportNoEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentReports_Employees_EmployeeId",
                table: "DeploymentReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentReports_Employees_ReplacementEmployeeId",
                table: "DeploymentReports");

            migrationBuilder.DropIndex(
                name: "IX_DeploymentReports_EmployeeId",
                table: "DeploymentReports");

            migrationBuilder.DropIndex(
                name: "IX_DeploymentReports_ReplacementEmployeeId",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "ReplacementEmployeeId",
                table: "DeploymentReports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "DeploymentReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReplacementEmployeeId",
                table: "DeploymentReports",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_EmployeeId",
                table: "DeploymentReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_ReplacementEmployeeId",
                table: "DeploymentReports",
                column: "ReplacementEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentReports_Employees_EmployeeId",
                table: "DeploymentReports",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentReports_Employees_ReplacementEmployeeId",
                table: "DeploymentReports",
                column: "ReplacementEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
