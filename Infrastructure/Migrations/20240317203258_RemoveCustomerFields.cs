using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclarationsOfAssignment",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsCareContractAvailable",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "DeclarationsOfAssignment",
                table: "Customers",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCareContractAvailable",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
