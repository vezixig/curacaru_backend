using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentSignatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSignedByCustomer",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsSignedByEmployee",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "SignatureCustomer",
                table: "Appointments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SignatureEmployee",
                table: "Appointments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureCustomer",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SignatureEmployee",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "IsSignedByCustomer",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSignedByEmployee",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
