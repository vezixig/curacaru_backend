using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StringLength3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ZipCities",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureManager",
                table: "WorkingTimeReports",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureEmployee",
                table: "WorkingTimeReports",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "Invoices",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureEmployee",
                table: "DeploymentReports",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureCustomer",
                table: "DeploymentReports",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "AssignmentDeclarations",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureEmployee",
                table: "Appointments",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureCustomer",
                table: "Appointments",
                type: "character varying(100000)",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15000)",
                oldMaxLength: 15000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ZipCities",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureManager",
                table: "WorkingTimeReports",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureEmployee",
                table: "WorkingTimeReports",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "Invoices",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureEmployee",
                table: "DeploymentReports",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureCustomer",
                table: "DeploymentReports",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "AssignmentDeclarations",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureEmployee",
                table: "Appointments",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "SignatureCustomer",
                table: "Appointments",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100000)",
                oldMaxLength: 100000);
        }
    }
}
