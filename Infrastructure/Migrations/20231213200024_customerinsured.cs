﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class customerinsured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsuredPersonNumber",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuredPersonNumber",
                table: "Customers");
        }
    }
}
