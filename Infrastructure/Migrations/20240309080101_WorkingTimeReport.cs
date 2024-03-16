using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkingTimeReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingTimeReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    SignatureEmployee = table.Column<string>(type: "text", nullable: false),
                    SignatureEmployeeCity = table.Column<string>(type: "text", nullable: false),
                    SignatureEmployeeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SignatureManager = table.Column<string>(type: "text", nullable: true),
                    SignatureManagerCity = table.Column<string>(type: "text", nullable: true),
                    SignatureManagerDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalHours = table.Column<double>(type: "double precision", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTimeReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingTimeReports_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingTimeReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingTimeReports_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTimeReports_CompanyId",
                table: "WorkingTimeReports",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTimeReports_EmployeeId_Year_Month",
                table: "WorkingTimeReports",
                columns: new[] { "EmployeeId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTimeReports_ManagerId",
                table: "WorkingTimeReports",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingTimeReports");
        }
    }
}
