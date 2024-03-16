using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentDeclaration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentDeclarations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerFirstName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerLastName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CustomerStreet = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CustomerZipCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    InsuranceId = table.Column<Guid>(type: "uuid", nullable: false),
                    InsuranceName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    InsuranceStreet = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    InsuranceZipCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    InsuredPersonNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Signature = table.Column<string>(type: "text", nullable: false),
                    SignatureCity = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SignatureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentDeclarations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentDeclarations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentDeclarations_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentDeclarations_ZipCities_CustomerZipCode",
                        column: x => x.CustomerZipCode,
                        principalTable: "ZipCities",
                        principalColumn: "ZipCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentDeclarations_ZipCities_InsuranceZipCode",
                        column: x => x.InsuranceZipCode,
                        principalTable: "ZipCities",
                        principalColumn: "ZipCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDeclarations_CustomerId_Year",
                table: "AssignmentDeclarations",
                columns: new[] { "CustomerId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDeclarations_CustomerZipCode",
                table: "AssignmentDeclarations",
                column: "CustomerZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDeclarations_InsuranceId",
                table: "AssignmentDeclarations",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentDeclarations_InsuranceZipCode",
                table: "AssignmentDeclarations",
                column: "InsuranceZipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentDeclarations");
        }
    }
}
