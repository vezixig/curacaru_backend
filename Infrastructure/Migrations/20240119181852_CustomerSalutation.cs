using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomerSalutation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Salutation",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salutation",
                table: "Customers");
        }
    }
}
