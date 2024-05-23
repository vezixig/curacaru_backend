using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContactForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FontSize = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactForms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactForms");
        }
    }
}
