using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeploymentInsurance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "DeploymentReports");

            migrationBuilder.AddColumn<Guid>(
                name: "InsuranceId",
                table: "DeploymentReports",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_InsuranceId",
                table: "DeploymentReports",
                column: "InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentReports_Insurances_InsuranceId",
                table: "DeploymentReports",
                column: "InsuranceId",
                principalTable: "Insurances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentReports_Insurances_InsuranceId",
                table: "DeploymentReports");

            migrationBuilder.DropIndex(
                name: "IX_DeploymentReports_InsuranceId",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "DeploymentReports");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceName",
                table: "DeploymentReports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
