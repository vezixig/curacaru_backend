using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeploymentReportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CareLevel",
                table: "DeploymentReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                table: "DeploymentReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "DeploymentReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceName",
                table: "DeploymentReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsuredPersonNumber",
                table: "DeploymentReports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareLevel",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "DeploymentReports");

            migrationBuilder.DropColumn(
                name: "InsuredPersonNumber",
                table: "DeploymentReports");
        }
    }
}
