using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeploymentReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeploymentReportId",
                table: "Appointments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeploymentReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClearanceType = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    ReplacementEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    SignatureCity = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SignatureCustomer = table.Column<string>(type: "text", nullable: false),
                    SignatureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SignatureEmployee = table.Column<string>(type: "text", nullable: false),
                    WorkedHours = table.Column<double>(type: "double precision", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeploymentReports_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeploymentReports_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeploymentReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeploymentReports_Employees_ReplacementEmployeeId",
                        column: x => x.ReplacementEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DeploymentReportId",
                table: "Appointments",
                column: "DeploymentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_CompanyId",
                table: "DeploymentReports",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_CustomerId_Year_Month_ClearanceType",
                table: "DeploymentReports",
                columns: new[] { "CustomerId", "Year", "Month", "ClearanceType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_EmployeeId",
                table: "DeploymentReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentReports_ReplacementEmployeeId",
                table: "DeploymentReports",
                column: "ReplacementEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DeploymentReports_DeploymentReportId",
                table: "Appointments",
                column: "DeploymentReportId",
                principalTable: "DeploymentReports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DeploymentReports_DeploymentReportId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "DeploymentReports");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DeploymentReportId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DeploymentReportId",
                table: "Appointments");
        }
    }
}
